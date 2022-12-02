using Microsoft.Extensions.Configuration;
using ModelsLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;

namespace DesktopApplication.Services
{
    public class APIService
    {
        private static readonly HttpClient _client = new();

        public static async Task<T?> GetAsync<T>(string url)
        {
            ConfigureClient();
            var result = await _client.GetAsync(url);

            result.EnsureSuccessStatusCode();

            string resultJson = await result.Content.ReadAsStringAsync();
            T? resultModel = JsonConvert.DeserializeObject<T>(resultJson);

            return resultModel;
        }

        public static async Task PostAsync<T>(string url, T contentValue)
        {
            ConfigureClient();
            var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync(url, content);

            result.EnsureSuccessStatusCode();
        }

        public static async Task PutAsync<T>(string url, T stringValue)
        {
            ConfigureClient();
            var content = new StringContent(JsonConvert.SerializeObject(stringValue), Encoding.UTF8, "application/json");
            var result = await _client.PutAsync(url, content);

            result.EnsureSuccessStatusCode();
        }

        public static async Task DeleteAsync(string url)
        {
            ConfigureClient();
            var result = await _client.DeleteAsync(url);

            result.EnsureSuccessStatusCode();
        }

        private static void ConfigureClient()
        {
            _client.BaseAddress = new Uri("https://www.budgethero.app/api/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }
    }
}
