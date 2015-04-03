using System.Net;
using System.Net.Http;
using System.Text;

namespace AddressValidation.Tests.MockData
{
    public static class MocksUsps
    {
        public static HttpResponseMessage MockResponse()
        {
            const string xml =
                "<AddressValidateResponse><Address ID=\"0\"><Address2>5720 1ST AVE S</Address2><City>BIRMINGHAM</City><State>AL</State><Zip5>35212</Zip5><Zip4>2522</Zip4><DeliveryPoint>20</DeliveryPoint><CarrierRoute>C018</CarrierRoute></Address></AddressValidateResponse>";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(xml, Encoding.UTF8, "text/xml")
            };
            return response;
        }

        public static HttpResponseMessage MockInvalidResponse()
        {
            const string xml =
                "<AddressValidateResponse><Address ID=\"0\"><Error><Number>-2147219399</Number><Source>clsAMS</Source><Description>Invalid Zip Code.  </Description><HelpFile/><HelpContext/></Error></Address></AddressValidateResponse>";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(xml, Encoding.UTF8, "text/xml")
            };
            return response;
        }
    }
}
