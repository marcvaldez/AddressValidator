using AddressValidation.Models;
using AddressValidation.XAVService;
using System.Collections.Generic;

namespace AddressValidation.Framework.UPS
{
    public static class UpsExtensions
    {
        public static Address ToAddress(this AddressKeyFormatType addressKeyFormat)
        {
            var address = new Address
            {
                Line = addressKeyFormat.AddressLine,
                City = addressKeyFormat.PoliticalDivision2,
                State = addressKeyFormat.PoliticalDivision1,
                Zip = addressKeyFormat.PostcodePrimaryLow,
                ZipExt = addressKeyFormat.PostcodeExtendedLow,
                Country = addressKeyFormat.CountryCode
            };

            return address;
        }

        public static AddressKeyFormatType ToAddressKeyFormat(this Address address)
        {
            var addressKeyFormat = new AddressKeyFormatType
            {
                AddressLine = address.Line,
                PoliticalDivision2 = address.City,
                PoliticalDivision1 = address.State,
                PostcodePrimaryLow = address.Zip,
                PostcodeExtendedLow = address.ZipExt,
                CountryCode = address.Country
            };

            return addressKeyFormat;
        }

        public static ValidationResult ToValidationResult(this XAVResponse response)
        {
            var result = new ValidationResult
            {
                Suggestions = new List<Address>(),
                Status = ServiceResultStatus.Ok
            };

            if (response.Response != null && response.Response.ResponseStatus.Code == "264003")
            {
                result.Status = ServiceResultStatus.AccessLimitExceeded;
                return result;
            }

            if (response.Candidate != null && response.Candidate.Length < 1)
            {
                result.Status = ServiceResultStatus.NotFound;
                return result;
            }

            foreach (var candidate in response.Candidate)
            {
                result.Suggestions.Add(candidate.AddressKeyFormat.ToAddress());
            }

            return result;
        }
    }
}