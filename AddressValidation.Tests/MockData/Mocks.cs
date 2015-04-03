using AddressValidation.Models;
using System.Collections.Generic;

namespace AddressValidation.Tests.MockData
{
    public static class Mocks
    {
        public static Address MockAddress()
        {
            return new Address
            {
                Line = new[] { "5720 1ST AVE S" },
                City = "BIRMINGHAM",
                State = "AL",
                Country = "US",
                Zip = "35212",
                ZipExt = "2522"
            };
        }

        public static ValidationResult MockResultAddress()
        {
            return new ValidationResult
            {
                ErrorMessage = "",
                Status = ServiceResultStatus.Ok,
                Suggestions = new List<Address> { MockAddress() }
            };
        }

        public static List<Address> MockAddresses()
        {
            return new List<Address> { MockAddress() };
        }

    }
}
