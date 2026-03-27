using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Servivce.HttpHelper.Extensions;

namespace Servivce.HttpHelper.HttpHelper;
using Microsoft.AspNetCore.Http;

public class HttpHelper
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HttpHelper> _logger;
    private readonly string _clientName = "DefaultHttpClient";

    public HttpHelper(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger<HttpHelper> logger)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }
    
    
    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient(_clientName);

        // Gắn Bearer token từ HttpContext
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
        }
        return client;
    }
    
    public async Task<TResult> GetAsync<TParam, TResult>(
        string url,
        TParam queryParams,
        CancellationToken ct = default)
    {
        try
        {
            // Build URL động từ UrlBuilder
            string fullUrl = UrlBuilder.Build(url, queryParams);

            // Gọi và parse JSON -> TResult
            return await SendWithRetry<TResult>(client => client.GetAsync(fullUrl, ct));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[HttpHelper] Error in GetAsync");
            throw e;
        }
    }
    
    public async Task<TResult> PostAsync<TParam, TResult>(
        string url,
        TParam contentValue,
        CancellationToken ct)
    {
        try
        {
            var jsonData = await PostRawAsync<TParam>(url, contentValue, ct);

            return JsonConvert.DeserializeObject<TResult>(jsonData);
        }
        catch
        {
            throw;
        }
    }
    
    public async Task<string?> PostRawAsync<TParam>(string url, TParam contentValue, CancellationToken ct)
    {
        try
        {
            var payload = JsonConvert.SerializeObject(contentValue);
            return await SendWithRetryRaw(async client =>
            {
                using var content = new StringContent(payload, Encoding.UTF8, "application/json");
                return await client.PostAsync(url, content, ct);
            });
        }
        catch
        {
            throw;
        }
    }
    
    public async Task<TResult> PatchAsync<TParam, TResult>(string url, TParam contentValue, CancellationToken ct)
    {
        try
        {
            var payload = JsonConvert.SerializeObject(contentValue);
            return await SendWithRetry<TResult>(async client =>
            {
                using var content = new StringContent(payload, Encoding.UTF8, "application/json");
                return await client.PatchAsync(url, content, ct);
            });
        }
        catch
        {
            throw;
        }
    }

    public async Task<TResult> PutAsync<TParam, TResult>(
        string url,
        TParam contentValue)
    {
        try
        {
            return await SendWithRetry<TResult>(async client =>
            {
                HttpContent? content = null;
                if (contentValue != null)
                {
                    var payload = JsonConvert.SerializeObject(contentValue);
                    content = new StringContent(payload, Encoding.UTF8, "application/json");
                }

                return await client.PutAsync(url, content);
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[HttpHelper] Error in PutAsync");
            throw;
        }
    }

    public async Task<TResult> DeleteAsync<TParam, TResult>(
        string url,
        TParam contentValue)
    {
        try
        {
            return await SendWithRetry<TResult>(async client => await client.DeleteAsync(url));
        }
        catch
        {
            throw;
        }
    }
    
    private async Task<string?> SendWithRetryRaw(
        Func<HttpClient, Task<HttpResponseMessage>> requestFactory)
    {
        try
        {
            var client = CreateClient();
            var response = await requestFactory(client);

            // if (response.StatusCode == HttpStatusCode.Unauthorized)
            // {
            //     var newToken = await _tokenRefresher.RefreshTokenAsync();
            //     if (!string.IsNullOrEmpty(newToken))
            //     {
            //         // update header trong HttpContext để lần sau CreateClient() sẽ có token mới
            //         _httpContextAccessor.HttpContext!.Request.Headers["Authorization"] = $"Bearer {newToken}";
            //
            //         client = CreateClient();
            //         response = await requestFactory(client);
            //     }
            // }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
        catch
        {
            throw;
        }
    }
    
    private async Task<TResult> SendWithRetry<TResult>(
        Func<HttpClient, Task<HttpResponseMessage>> requestFactory)
    {
        try
        {
            var json = await SendWithRetryRaw(requestFactory);

            // Ở đây ta giữ behaviour cũ:
            // assume json hợp lệ và không null.
            return JsonConvert.DeserializeObject<TResult>(json!)!;
        }
        catch
        {
            throw;
        }
    }
}