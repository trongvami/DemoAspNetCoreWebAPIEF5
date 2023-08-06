using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using NuGet.Common;
using System.Security.Policy;
using System.Text.Json.Serialization;

namespace WebApplication1.Helpers
{
    public static class HttpClientHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<T> Post2Async<T>(string url, T data)
        {
            var httpClient = new HttpClient();
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseString);
            }

            return default;
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(string url, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, content);
        }

        public static async Task<HttpResponseMessage> PostWithTokenAsync<T>(string url, T data, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, content);
        }
        
        public static async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public static async Task<List<T>> GetListAsync<T>(string url, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public static async Task<HttpResponseMessage> PutWithTokenAsync<T>(string url, T data, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync(url, content);
        }

        public static async Task<HttpResponseMessage> PutAsync<T>(string url, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync(url, content);
        }

        public static async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await _httpClient.DeleteAsync(url);
        }

        public static async Task<HttpResponseMessage> DeleteWithTokenAsync(string url, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.DeleteAsync(url);
        }

        public static async Task<HttpResponseMessage> DeleteWithTokenAndIdAsync(string url, string token, string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.DeleteAsync(url + id);
        }
    }
}
