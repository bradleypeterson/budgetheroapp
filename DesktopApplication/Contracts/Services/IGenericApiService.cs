using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Contracts.Services
{
    public interface IGenericApiService
    {
        public Task<T?> GetAsync<T>(string url);
        public Task<int> PostAsync<T>(string url, T contentValue);
        public Task<int> PutAsync<T>(string url, T stringValue);
        public Task DeleteAsync(string url);
    }
}
