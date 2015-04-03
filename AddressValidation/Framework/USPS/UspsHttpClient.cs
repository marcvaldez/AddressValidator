using AddressValidation.Framework.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AddressValidation.Framework.USPS
{
    public class UspsHttpClient : HttpClient, IHttpClient
    {
        public UspsHttpClient()
        {
            BaseAddress = new Uri("http://production.shippingapis.com");
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}