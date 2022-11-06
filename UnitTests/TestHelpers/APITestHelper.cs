using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.TestHelpers
{
    public class APITestHelper
    {
        private static readonly HttpClient _client = new();

        public static async Task<bool> GetAsync<T>(string url)
        {
            ConfigureClient();
            var result = await _client.GetAsync(url);

            if (!result.IsSuccessStatusCode)
                return false;

            var resultJson = await result.Content.ReadAsStringAsync();
            T resultModel = JsonConvert.DeserializeObject<T>(resultJson);

            if (resultModel is not null)
                return true;
            else
                return false;

        }

        public static async Task<bool> PostAsync<T>(string url, T contentValue)
        {
            ConfigureClient();
            var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync(url, content);

            if (result.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public static async Task<bool> PutAsync<T>(string url, T stringValue)
        {
            ConfigureClient();
            var content = new StringContent(JsonConvert.SerializeObject(stringValue), Encoding.UTF8, "application/json");
            var result = await _client.PutAsync(url, content);

            if (result.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public static async Task<bool> DeleteAsync(string url)
        {
            ConfigureClient();
            var result = await _client.DeleteAsync(url);

            if (result.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        private static void ConfigureClient()
        {
            var apiBaseAddAddress = "https://www.budgethero.app/api/";
            _client.BaseAddress = new Uri(apiBaseAddAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }
    }
}
