using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.HttpApiManager
{
    public interface IApiManager
    {
        Task<T> GetAsync<T>(string uri);
        Task<T> GetAsync<T>(string uri, object data);
        Task<T> GetAsync<T>(string uri, T data);
        Task<T> PostAsync<T>(string uri, T data);
        Task<T> PostAsync<T>(string uri, object data);
        Task<T> PutAsync<T>(string uri, object data);
        Task<T> DeleteAsync<T>(string uri);
        Task<R> PostAsync<T, R>(string uri, T data);
        Task<R> PutAsync<T, R>(string uri, T data);
        Task<R> DeleteAsync<T, R>(string uri, T data);
    }
}
