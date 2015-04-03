using AddressValidation.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AddressValidation.Framework.USPS
{
    public static class UspsExtensions
    {
        public async static Task<ValidationResult> ToValidationResultAsync(this HttpResponseMessage response)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(await response.Content.ReadAsStreamAsync());
            var error = xmlDoc.SelectSingleNode("/AddressValidateResponse/Address/Error") ??
                        xmlDoc.SelectSingleNode("/Error");

            return error != null ? error.ToErrorValidationResult() : xmlDoc.ToValidationResult();
        }

        private static ValidationResult ToErrorValidationResult(this XmlNode error)
        {
            return (new ValidationResult
            {
                Status = ServiceResultStatus.InvalidRequest,
                ErrorMessage = error.ToString("Description")
            });
        }

        private static ValidationResult ToValidationResult(this XmlNode node)
        {
            var result = new ValidationResult
            {
                Suggestions = new List<Address>(),
                Status = ServiceResultStatus.Ok
            };

            var matches = node.SelectNodes("/AddressValidateResponse/Address");
            if (matches == null)
            {
                result.Status = ServiceResultStatus.NotFound;
                return result;
            }

            foreach (XmlNode match in matches)
            {
                result.Suggestions.Add(new Address
                {
                    City = match.ToString("City"),
                    State = match.ToString("State"),
                    Zip = match.ToString("Zip5"),
                    ZipExt = match.ToString("Zip4"),
                    Country = "US",
                    Line = match.ToString("Address1") != string.Empty //Address2 is the street address
                        ? new[] { match.ToString("Address2"), match.ToString("Address1") }
                        : new[] { match.ToString("Address2") }
                });
            }

            return result;
        }

        public static string ToXmlString(this Address address, string uspsUserId)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("<AddressValidateRequest USERID=\"{0}\">", uspsUserId);
            sb.Append("<IncludeOptionalElements>true</IncludeOptionalElements>");
            sb.Append("<ReturnCarrierRoute>true</ReturnCarrierRoute>");
            sb.Append("<Address ID=\"0\">");
            sb.Append(XmlOrEmpty("Address1", address.Line.Length > 0 ? address.Line[0] : null));
            sb.Append(XmlOrEmpty("Address2", address.Line.Length > 1 ? address.Line[1] : null));
            sb.Append(XmlOrEmpty("City", address.City));
            sb.Append(XmlOrEmpty("State", address.State));
            sb.Append(XmlOrEmpty("Zip5", address.Zip));
            sb.Append(XmlOrEmpty("Zip4", address.ZipExt));
            sb.Append("</Address>");
            sb.Append("</AddressValidateRequest>");
            return sb.ToString();
        }

        private static string XmlOrEmpty(string field, string val)
        {
            return !string.IsNullOrEmpty(val)
                ? string.Format("<{0}>{1}</{0}>", field, val)
                : string.Format("<{0} />", field);
        }

        private static string ToString(this XmlNode doc, string field)
        {
            if (doc == null) return string.Empty;
            return doc[field] != null ? doc[field].InnerText : string.Empty;
        }
    }
}