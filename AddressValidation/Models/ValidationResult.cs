using System.Collections.Generic;

namespace AddressValidation.Models
{
    public class ValidationResult
    {
        public ServiceResultStatus Status { get; set; }
        public List<Address> Suggestions;
        public string ErrorMessage;
    }

    public enum ServiceResultStatus
    {
        Ok,
        NotFound,
        InvalidRequest,
        ServiceUnavailable,
        AccessLimitExceeded
    }
}