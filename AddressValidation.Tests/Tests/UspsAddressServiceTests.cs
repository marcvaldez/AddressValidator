using AddressValidation.Framework.Interfaces;
using AddressValidation.Framework.USPS;
using AddressValidation.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AddressValidation.Tests.MockData;
using Moq;
using Mocks = AddressValidation.Tests.MockData.Mocks;


namespace AddressValidation.Tests.Tests
{
    [TestClass]
    public class UspsAddressServiceTests
    {
        [TestMethod]
        public void ValidateAddress_ShouldReturnCorrectValidateResult()
        {
            var client = new Mock<IHttpClient>();
            client.Setup(c => c.GetAsync(It.IsAny<String>())).ReturnsAsync(MocksUsps.MockResponse());

            var service = new UspsAddressService(client.Object);
            var result = service.ValidateAddress(Mocks.MockAddress(), 5).Result;

            CollectionAssert.AreEqual(Mocks.MockAddresses(), result.Suggestions);
            Assert.AreEqual(ServiceResultStatus.Ok, result.Status);
        }

        [TestMethod]
        public void ValidateAddress_ShouldReturnValidateResultOnException()
        {
            var client = new Mock<IHttpClient>();
            client.Setup(c => c.GetAsync(It.IsAny<String>())).Throws(new Exception("mock exception"));

            var service = new UspsAddressService(client.Object);
            var result = service.ValidateAddress(Mocks.MockAddress(), 5).Result;

            Assert.AreEqual(ServiceResultStatus.ServiceUnavailable, result.Status);
            Assert.AreEqual("mock exception", result.ErrorMessage);
        }
    }
}
