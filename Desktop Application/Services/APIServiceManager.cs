using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Application.Services
{
    public class APIServiceManager
    {
        private static HttpClient client = new();

        public static async Task<User> GetUserAsync(User user)
        {
            ConfigureClient();

            string endpoint = $"users/{user.UserId}";
            User requestedUser = null;
            HttpResponseMessage response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                requestedUser = await response.Content.ReadAsAsync<User>();
            }

            return requestedUser;
        }

        private static void ConfigureClient()
        {
            client.BaseAddress = new Uri("https://www.budgethero.app/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }
    }
}
