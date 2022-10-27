using ModelsLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Application.Services
{
    public class APIServiceManager
    {
        private static readonly HttpClient _client = new();

        public static async Task<T> GetAsync<T>(string url)
        {
            ConfigureClient();
            var result = await _client.GetAsync(url);

            result.EnsureSuccessStatusCode();

            string resultJson = await result.Content.ReadAsStringAsync();
            T resultModel = JsonConvert.DeserializeObject<T>(resultJson);

            return resultModel;
        }

        public static async Task PostAsync<T>(string url, T contentValue)
        {
            ConfigureClient();
            var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8);
            var result = await _client.PostAsync(url, content);

            result.EnsureSuccessStatusCode();
        }

        public static async Task PutAsync<T>(string url, T stringValue)
        {
            ConfigureClient();
            var content = new StringContent(JsonConvert.SerializeObject(stringValue), Encoding.UTF8);
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
            string apiBaseAddAddress = ConfigurationManager.AppSettings["apiBaseAddress"];
            _client.BaseAddress = new Uri(apiBaseAddAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }
    }
}
