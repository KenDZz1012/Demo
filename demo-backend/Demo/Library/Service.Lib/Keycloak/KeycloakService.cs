using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lib.Keycloak
{
    public class KeycloakService : IKeycloakService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Environment.GetEnvironmentVariable("KEYCLOAK_URL");
        private readonly string _realm = Environment.GetEnvironmentVariable("KEYCLOAK_REALM");
        private readonly string _adminUsername = Environment.GetEnvironmentVariable("KEYCLOAK_ADMIN_USERNAME");
        private readonly string _adminPassword = Environment.GetEnvironmentVariable("KEYCLOAK_ADMIN_PASSWORD");

        public KeycloakService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gán role cho user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task AssignRoleAsync(string userId, string roleName)
        {
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get realm role
            var roleResp = await _httpClient.GetAsync($"{_baseUrl}/admin/realms/{_realm}/roles/{roleName}");
            roleResp.EnsureSuccessStatusCode();
            var roleJson = await roleResp.Content.ReadAsStringAsync();
            var role = JsonConvert.DeserializeObject<dynamic>(roleJson);

            var assignPayload = JsonConvert.SerializeObject(new[] { role });
            var assignContent = new StringContent(assignPayload, Encoding.UTF8, "application/json");

            var assignResp = await _httpClient.PostAsync(
                $"{_baseUrl}/admin/realms/{_realm}/users/{userId}/role-mappings/realm",
                assignContent
            );

            assignResp.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Tạo user mới keycloak
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task CreateUserAsync(KeycloakUserDto userDto)
        {
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var payload = new
            {
                username = userDto.UserName,
                email = userDto.Email,
                firstName = userDto.FirstName,
                lastName = userDto.LastName,
                enabled = true,
                credentials = new[]
                {
                    new
                    {
                        type = "password",
                        value = userDto.Password,
                        temporary = false
                    }
                }
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/admin/realms/{_realm}/users", content);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Lấy access token ADMIN
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessTokenAsync()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "admin-cli"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", _adminUsername),
                new KeyValuePair<string, string>("password", _adminPassword)
            });

            var response =
                await _httpClient.PostAsync($"{_baseUrl}/realms/master/protocol/openid-connect/token", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(json);
            return result.access_token;
        }

        /// <summary>
        /// Cập nhật mật khẩu cho user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdatePasswordAsync(string userID, string newPassword)
        {
            var token = await GetAccessTokenAsync();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var body = new
            {
                type = "password",
                temporary = false,
                value = newPassword
            };
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/admin/realms/{_realm}/users/{userID}/reset-password",
                content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to update password in Keycloak");
            }
        }

        /// <summary>
        /// Lấy UserID trong keycloak
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<string?> GetUserIdByUsernameAsync(string username)
        {
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}/admin/realms/{_realm}/users?username={username}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<dynamic>>(json);

            if (users != null && users.Count > 0)
            {
                return users[0].id;
            }

            return null;
        }

        /// <summary>
        /// Lấy token đăng nhập user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GetUserTokenAsync(string userName, string password)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "public-client"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
            });
            var response =
                await _httpClient.PostAsync($"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Lỗi lấy user token từ Keycloak: {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(json);

            return result?.access_token;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/admin/realms/{_realm}/users/{userId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<(string AccessToken, string RefreshToken)> GetUserTokenWithRefreshAsync(string username,
            string password)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "public-client"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            var response = await _httpClient.PostAsync($"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token", content);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return (null, null);
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Keycloak returned {response.StatusCode}: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);
            string accessToken = result?.access_token;
            string refreshToken = result?.refresh_token;

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                throw new InvalidOperationException("Failed to retrieve tokens from Keycloak response");
            }

            return (accessToken, refreshToken);
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "public-client"),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            });

            var response =
                await _httpClient.PostAsync($"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token", content);
            if (!response.IsSuccessStatusCode) return (null, null);

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);

            return ((string)result.access_token, (string)result.refresh_token);
        }
    }
}