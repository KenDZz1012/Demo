using Microsoft.Extensions.Logging;
using Service.Lib.BaseResponse;
using Servivce.HttpHelper.Dtos.Authorize;
using Servivce.HttpHelper.HttpHelper;

namespace Servivce.HttpHelper.Services;

public class AuthorizeHttpService
{
    private readonly HttpHelper.HttpHelper _httpHelper;
    private readonly string? _baseUrl;
    private readonly ILogger<AuthorizeHttpService> _logger;

    public AuthorizeHttpService(HttpHelper.HttpHelper httpHelper, ILogger<AuthorizeHttpService> logger)
    {
        _httpHelper = httpHelper;
        _baseUrl = Environment.GetEnvironmentVariable("AUTHORIZE_URL");
        _logger = logger;
    }

    /// <summary>
    /// Gọi POST <c>{AUTHORIZE_URL}/v1/users</c> — khớp <see cref="Authorize.Controllers.AuthorizeController.CreateUser"/>.
    /// </summary>
    public async Task<ApiResponse<Guid>?> CreateIdentityUserAsync(CreateIdentityUserHttpDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl))
        {
            _logger.LogError("AUTHORIZE_URL is not configured.");
            return ApiResponse<Guid>.Failure("500", "Chưa cấu hình AUTHORIZE_URL.");
        }
        try
        {
            var result = await _httpHelper.PostAsync<CreateIdentityUserHttpDto, ApiResponse<Guid>>($"{_baseUrl}/v1/users", dto, ct);
            return result ?? ApiResponse<Guid>.Failure("502", "Authorize trả về rỗng.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateIdentityUserAsync failed");
            return ApiResponse<Guid>.Failure("502", ex.Message);
        }
    }
}
