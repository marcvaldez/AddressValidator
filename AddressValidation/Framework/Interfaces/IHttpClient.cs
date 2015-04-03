using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AddressValidation.Framework.Interfaces
{
    public interface IHttpClient : IDisposable
    {
        Task<HttpResponseMessage> GetAsync(string query);
    }
}
