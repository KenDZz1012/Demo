using Microsoft.Extensions.Logging;
using Service.Lib.BaseResponse;
using Servivce.HttpHelper.Dtos.Account;
using Servivce.HttpHelper.HttpHelper;
namespace Servivce.HttpHelper.Services;

public class AccountHttpService
{
    private readonly HttpHelper.HttpHelper _httpHelper;
    private readonly string _baseUrl;
    private readonly ILogger<AccountHttpService> _logger; 
    
    public AccountHttpService(HttpHelper.HttpHelper httpHelper, ILogger<AccountHttpService> logger)
    {
        _httpHelper = httpHelper;
        _baseUrl = Environment.GetEnvironmentVariable("ACCOUNT_URL");
        _logger = logger;
    }


    public async Task<UserHttpDto> GetUserInfoAsync(string userId, CancellationToken ct  = default)
    {
        try
        {
            var userInfo = await _httpHelper.GetAsync<object, ApiResponse<UserHttpDto>>($"{_baseUrl}/users/{userId}",null, ct);
            return userInfo.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return null;
        }
    }
}