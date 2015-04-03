using AddressValidation.Framework.Interfaces;
using AddressValidation.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AddressValidation.Controllers
{
    public class AddressController : ApiController
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<IEnumerable<Address>> Get([FromUri] Address address, int maxSuggestions)
        {
            var result = await _addressService.ValidateAddress(address, maxSuggestions);

            switch (result.Status)
            {
                case ServiceResultStatus.Ok:
                    return result.Suggestions;
                case ServiceResultStatus.NotFound:
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("No matches found for address")
                    });
                case ServiceResultStatus.InvalidRequest:
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Invalid request: " + result.ErrorMessage)
                    });
                case ServiceResultStatus.AccessLimitExceeded:
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("Access limit exceeded")
                    });
                case ServiceResultStatus.ServiceUnavailable:
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                    {
                        Content = new StringContent("An error occurred while calling service: " + result.ErrorMessage)
                    });
            }
            return result.Suggestions;
        }

    }
}
