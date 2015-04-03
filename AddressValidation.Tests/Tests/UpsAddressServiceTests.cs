using System;
using AddressValidation.Framework.Interfaces;
using AddressValidation.Framework.UPS;
using AddressValidation.Models;
using AddressValidation.Tests.MockData;
using AddressValidation.XAVService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mocks = AddressValidation.Tests.MockData.Mocks;

namespace AddressValidation.Tests.Tests
{
    [TestClass]
    public class UpsAddressServiceTests
    {
        [TestMethod]
        public void ValidateAddress_ShouldReturnCorrectValidateResult()
        {
            var addressService = SetupAddressServiceWithMockClient(MocksUps.MockXavResponse());

            var result = addressService.ValidateAddress(Mocks.MockAddress(), 5).Result;

            CollectionAssert.AreEqual(Mocks.MockAddresses(), result.Suggestions);
            Assert.AreEqual(ServiceResultStatus.Ok, result.Status);
        }

        [TestMethod]
        public void ValidateAddress_ShouldReturnNotFound()
        {
            var addressService = SetupAddressServiceWithMockClient(MocksUps.MockXavResponseNoCandidates());

            var result = addressService.ValidateAddress(Mocks.MockAddress(), 5).Result;

            Assert.AreEqual(ServiceResultStatus.NotFound, result.Status);
        }

        [TestMethod]
        public void ValidateAddress_ShouldReturnLimitExceeded()
        {
            var addressService = SetupAddressServiceWithMockClient(MocksUps.MockXavResponseLimitExceeded());

            var result = addressService.ValidateAddress(Mocks.MockAddress(), 5).Result;

            Assert.AreEqual(ServiceResultStatus.AccessLimitExceeded, result.Status);
        }

        [TestMethod]
        public void ValidateAddress_ShouldReturnValidateResultOnException()
        {
            var client = new Mock<IXavPortTypeClient>();
            client.Setup(c => c.ProcessXAVAsync(It.IsAny<UPSSecurity>(), It.IsAny<XAVRequest>()))
                .Throws(new Exception("mock exception"));

            var addressService = new UpsAddressService(client.Object);

            var result = addressService.ValidateAddress(Mocks.MockAddress(), 5).Result;

            Assert.AreEqual(ServiceResultStatus.ServiceUnavailable, result.Status);
            Assert.AreEqual("mock exception", result.ErrorMessage);
        }

        #region Setup
        private static UpsAddressService SetupAddressServiceWithMockClient(XAVResponse mockXavResponse)
        {
            var client = new Mock<IXavPortTypeClient>();
            client.Setup(c => c.ProcessXAVAsync(It.IsAny<UPSSecurity>(), It.IsAny<XAVRequest>()))
                .Raises(m => m.ProcessXAVCompleted += null,
                    new ProcessXAVCompletedEventArgs(new object[] { mockXavResponse }, null, false, null));

            return new UpsAddressService(client.Object);
        }
        #endregion
    }
}
