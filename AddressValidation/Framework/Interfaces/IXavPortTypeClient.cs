using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressValidation.XAVService;

namespace AddressValidation.Framework.Interfaces
{
    public interface IXavPortTypeClient
    {
        void ProcessXAVAsync(UPSSecurity upsSecurity, XAVRequest xavRequest);
        event EventHandler<ProcessXAVCompletedEventArgs> ProcessXAVCompleted;
    }
}
