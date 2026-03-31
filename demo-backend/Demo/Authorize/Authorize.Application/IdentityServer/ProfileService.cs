using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Servivce.HttpHelper.Constants.Account;
using Servivce.HttpHelper.Services;

namespace Authorize.Application.IdentityServer;

public class ProfileService : IProfileService
{
    private readonly AccountHttpService _accountHttpService;
    private readonly UserManager<ApplicationUser> _userManager;

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        var roles = await _userManager.GetRolesAsync(user);

        var userInfo = await _accountHttpService.GetUserInfoAsync(user.Id);
        context.IssuedClaims.AddRange(new[]
        {
            new Claim("display_name", userInfo.DisplayName),
            new Claim("avatar_url",   userInfo.AvatarUrl),
        });
        foreach (var role in roles)
            context.IssuedClaims.Add(new Claim("role", role));
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        var userInfo = await _accountHttpService.GetUserInfoAsync(user?.Id);
        if (user == null) return;
        context.IsActive = userInfo.Status == StatusConstant.ACTIVE;
    }
}