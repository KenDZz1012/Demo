using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lib.HttpRequest
{
    public class HttpRequestService : IHttpRequestService
    {
        private readonly HttpClient _httpClient;
        private readonly string urlApi = Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost:8000/api";
        public HttpRequestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> GetAsync<TResponse>(string endpoint, string token) where TResponse : new()
        {
            var result = new TResponse();

            try
            {
                _httpClient.BaseAddress = new Uri(urlApi);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", token);
                }

                var response = await _httpClient.GetAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    typeof(TResponse).GetProperty("StatusCode")?.SetValue(result, 401);
                }
                else
                {
                    result = JsonConvert.DeserializeObject<TResponse>(content);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
    }
}
