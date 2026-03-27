using Microsoft.Extensions.Logging;

namespace Servivce.HttpHelper.Services;

public class AccountHttpService
{
    private readonly HttpClient _httpHelper;
    private readonly string _baseUrl;
    private readonly ILogger<AccountHttpService> _logger; 
    
    public AccountHttpService(HttpClient httpHelper, ILogger<AccountHttpService> logger)
    {
        _httpHelper = httpHelper;
        _baseUrl = "http://localhost:5000/v1/account";
        _logger = logger;
    }
}