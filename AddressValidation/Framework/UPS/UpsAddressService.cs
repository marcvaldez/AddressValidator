using AddressValidation.Framework.Interfaces;
using AddressValidation.Models;
using AddressValidation.XAVService;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace AddressValidation.Framework.UPS
{
    public class UpsAddressService : IAddressService
    {
        private readonly IXavPortTypeClient _xavClient;
        public UpsAddressService(IXavPortTypeClient xavClient)
        {
            _xavClient = xavClient;
        }

        public async Task<ValidationResult> ValidateAddress(Address address, int maxSuggestions)
        {
            var tcs = new TaskCompletionSource<XAVResponse>();

            var xavRequest = CreateXavRequest(address, maxSuggestions);

            ValidationResult validationResult;
            _xavClient.ProcessXAVCompleted += (s, e) =>
            {
                if (e.Error != null) 
                    tcs.TrySetException(e.Error);
                else if (e.Cancelled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(e.Result);
            };

            try
            {
                _xavClient.ProcessXAVAsync(GetSecurity(), xavRequest);
                var xavResponse = await tcs.Task;
                validationResult = xavResponse.ToValidationResult();
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

        private static XAVRequest CreateXavRequest(Address address, int maxSuggestions)
        {
            var xavRequest = new XAVRequest
            {
                AddressKeyFormat = address.ToAddressKeyFormat(),
                MaximumCandidateListSize = maxSuggestions.ToString(),
                Request = new RequestType
                {
                    RequestOption = new[] {"1"}
                }
            };
            return xavRequest;
        }

        private static UPSSecurity GetSecurity()
        {
            return new UPSSecurity
            {
                ServiceAccessToken =
                    new UPSSecurityServiceAccessToken
                    {
                        AccessLicenseNumber = ConfigurationManager.AppSettings["UPSLicenseKey"]
                    },
                UsernameToken =
                    new UPSSecurityUsernameToken
                    {
                        Username = ConfigurationManager.AppSettings["UPSUser"],
                        Password = ConfigurationManager.AppSettings["UPSPass"]
                    }
            };
        }

    }
}