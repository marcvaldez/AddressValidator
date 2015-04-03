using System.Linq;
using System.Net;
using System.Web.Http;
using AddressValidation.Controllers;
using AddressValidation.Framework.Interfaces;
using AddressValidation.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mocks = AddressValidation.Tests.MockData.Mocks;

namespace AddressValidation.Tests.Tests
{
    [TestClass]
    public class AddressControllerTests
    {
        [TestMethod]
        public void Get_ShouldReturnSampleResponse()
        {
            var controller = CreateAddressControllerWithMockService(Mocks.MockResultAddress());

            var result = controller.Get(Mocks.MockAddress(), 5).Result.ToList();
            var expected = Mocks.MockAddresses();

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Get_ShouldReturnNotFound()
        {
            var ex = TestThrowsException(ServiceResultStatus.NotFound);

            Assert.AreEqual("No matches found for address", ex.Response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.NotFound, ex.Response.StatusCode);
        }

        [TestMethod]
        public void Get_ShouldReturnInvalid()
        {
            var ex = TestThrowsException(ServiceResultStatus.InvalidRequest);

            Assert.IsTrue(ex.Response.Content.ReadAsStringAsync().Result.Contains("Invalid request: "));
            Assert.AreEqual(HttpStatusCode.BadRequest, ex.Response.StatusCode);
        }

        [TestMethod]
        public void Get_ShouldReturnAccessLimitExceeded()
        {
            var ex = TestThrowsException(ServiceResultStatus.AccessLimitExceeded);

            Assert.AreEqual("Access limit exceeded", ex.Response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.Forbidden, ex.Response.StatusCode);
        }

        [TestMethod]
        public void Get_ShouldReturnUnavailable()
        {
            var ex = TestThrowsException(ServiceResultStatus.ServiceUnavailable);

            Assert.IsTrue(
                ex.Response.Content.ReadAsStringAsync().Result.Contains("An error occurred while calling service: "));
            Assert.AreEqual(HttpStatusCode.ServiceUnavailable, ex.Response.StatusCode);
        }

        #region Setup

        private static HttpResponseException TestThrowsException(ServiceResultStatus mockStatusResult)
        {
            var controller =
                CreateAddressControllerWithMockService(new ValidationResult { Status = mockStatusResult });
            var task = controller.Get(Mocks.MockAddress(), 5);

            // To ensure that our exception won't be wrapped in an AggregateExcception, we use task.GetAwaiter().GetResult()
            return ExceptionAssert.Throws<HttpResponseException>(() => { task.GetAwaiter().GetResult(); });
        }

        private static AddressController CreateAddressControllerWithMockService(ValidationResult shouldReturn)
        {
            var addressService = new Mock<IAddressService>();
            addressService.Setup(a => a.ValidateAddress(It.IsAny<Address>(), It.IsAny<int>()))
                .ReturnsAsync(shouldReturn);

            return new AddressController(addressService.Object);
        }

        #endregion
    }
}
