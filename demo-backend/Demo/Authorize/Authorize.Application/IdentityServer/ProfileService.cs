using System.Security.Claims;
using Authorize.Domain.Entities;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Servivce.HttpHelper.Constants.Account;
using Servivce.HttpHelper.Services;

namespace Authorize.Application.IdentityServer;

/// <summary>
/// Tùy chỉnh claims đưa vào access/id token (display name, avatar, role) và kiểm tra user còn active (theo Account).
/// Được đăng ký trong <c>AddProfileService&lt;ProfileService&gt;()</c>.
/// </summary>
public class ProfileService : IProfileService
{
    private readonly AccountHttpService _accountHttpService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ProfileService> _logger;

    public ProfileService(
        UserManager<ApplicationUser> userManager,
        AccountHttpService accountHttpService,
        ILogger<ProfileService> logger)
    {
        _userManager = userManager;
        _accountHttpService = accountHttpService;
        _logger = logger;
    }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            if (user == null)
                return;

            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = await _accountHttpService.GetUserInfoAsync(user.Id.ToString());
            if (userInfo != null)
            {
                context.IssuedClaims.AddRange(new[]
                {
                    new Claim("display_name", userInfo.DisplayName ?? string.Empty),
                    new Claim("avatar_url", userInfo.AvatarUrl ?? string.Empty),
                });
            }

            foreach (var role in roles)
                context.IssuedClaims.Add(new Claim("role", role));
        }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        if (user == null)
        {
            context.IsActive = false;
            return;
        }

        var userInfo = await _accountHttpService.GetUserInfoAsync(user.Id.ToString());
        if (userInfo != null)
        {
            var active = string.Equals(userInfo.Status, StatusConstant.ACTIVE, StringComparison.OrdinalIgnoreCase);
            context.IsActive = active;
            if (!active)
                _logger.LogWarning("User {UserId} inactive per Account API (Status={Status}).", user.Id, userInfo.Status);
            return;
        }

        // Account chưa có user cùng Id, API lỗi, hoặc chưa đồng bộ — dùng trạng thái trên Identity.
        var fallback = string.Equals(user.AccountStatus, "active", StringComparison.OrdinalIgnoreCase);
        context.IsActive = fallback;
        _logger.LogInformation(
            "Account user not resolved for {UserId}; IsActive from Identity AccountStatus={AccountStatus} => {Active}.",
            user.Id,
            user.AccountStatus,
            fallback);
    }
}
