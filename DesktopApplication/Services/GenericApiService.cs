using DebugTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Uri = System.Uri;

namespace DesktopApplication.Services
{
    public class GenericApiService
    {
        private readonly HttpClient _client = new();

        public GenericApiService()
        {
            ConfigureClient();
            ConfigureJsonSettings();
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            var result = await _client.GetAsync(url);

            result.EnsureSuccessStatusCode();

            string resultJson = await result.Content.ReadAsStringAsync();
            T? resultModel = JsonConvert.DeserializeObject<T>(resultJson);

            return resultModel;
        }

        public async Task<int> PostAsync<T>(string url, T contentValue)
        {
            var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync(url, content);
            
            if (result.IsSuccessStatusCode)
                return 1;
            else
                return 0;
        }

        public async Task<int> PutAsync<T>(string url, T stringValue)
        {
            var content = new StringContent(JsonConvert.SerializeObject(stringValue), Encoding.UTF8, "application/json");
            var result = await _client.PutAsync(url, content);

            if (result.IsSuccessStatusCode)
                return 1;
            else
                return 0;
        }

        public async Task DeleteAsync(string url)
        {
            var result = await _client.DeleteAsync(url);

            result.EnsureSuccessStatusCode();
        }

        private void ConfigureClient()
        {
            _client.BaseAddress = new Uri("https://www.budgethero.app/api/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        private void ConfigureJsonSettings()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
            };
        }
    }
}
