using System.Threading.Tasks;
using AddressValidation.Models;

namespace AddressValidation.Framework.Interfaces
{
    public interface IAddressService
    {
        Task<ValidationResult> ValidateAddress(Address address, int maxSuggestions);
    }
}
