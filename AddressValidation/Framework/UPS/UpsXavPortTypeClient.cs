using AddressValidation.Framework.Interfaces;
using AddressValidation.XAVService;
using System;

namespace AddressValidation.Framework.UPS
{
    public class UpsXavPortTypeClient: XAVPortTypeClient, IXavPortTypeClient
    {
        public new void ProcessXAVAsync(UPSSecurity upsSecurity, XAVRequest xavRequest)
        {
            base.ProcessXAVAsync(upsSecurity, xavRequest);
        }

        public new event EventHandler<ProcessXAVCompletedEventArgs> ProcessXAVCompleted;
    }
}