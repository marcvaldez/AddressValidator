using AddressValidation.Framework.Interfaces;
using AddressValidation.Models;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace AddressValidation.Framework.USPS
{
    public class UspsAddressService : IAddressService
    {
        private readonly IHttpClient _httpClient;

        public UspsAddressService(IHttpClient client)
        {
            _httpClient = client;
        }

        public async Task<ValidationResult> ValidateAddress(Address address, int maxSuggestions)
        {
            using (_httpClient)
            {
                ValidationResult validationResult;
                try
                {
                    var response =
                        await
                            _httpClient.GetAsync("ShippingAPITest.dll?API=Verify&XML=" +
                                                 address.ToXmlString(ConfigurationManager.AppSettings["USPSApiKey"]));
                    validationResult = await response.ToValidationResultAsync();
                }
                catch (Exception ex)
                {
                    validationResult = new ValidationResult
                    {
                        Status = ServiceResultStatus.ServiceUnavailable,
                        ErrorMessage = ex.Message
                    };
                }

                return validationResult;
            }

        }

    }
}