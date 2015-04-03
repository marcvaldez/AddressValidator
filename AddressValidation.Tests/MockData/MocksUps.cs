using AddressValidation.Framework.UPS;
using AddressValidation.XAVService;

namespace AddressValidation.Tests.MockData
{
    public static class MocksUps
    {
        public static XAVResponse MockXavResponse()
        {
            return new XAVResponse
            {
                Candidate = new[] { new CandidateType { AddressKeyFormat = MockAddressKeyFormat() } }
            };
        }

        public static XAVResponse MockXavResponseNoCandidates()
        {
            return new XAVResponse
            {
                Candidate = new CandidateType[] { }
            };
        }

        public static XAVResponse MockXavResponseLimitExceeded()
        {
            return new XAVResponse
            {
                Response = new ResponseType {ResponseStatus = new CodeDescriptionType {Code = "264003"}}
            };
        }

        public static AddressKeyFormatType MockAddressKeyFormat()
        {
            return Mocks.MockAddress().ToAddressKeyFormat();
        }
    }
}
