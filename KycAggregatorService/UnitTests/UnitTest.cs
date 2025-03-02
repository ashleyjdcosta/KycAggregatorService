using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KycAggregatorService.Services;

namespace KycAggregatorService.UnitTests
{
    [TestClass]
    public class CustomerServiceTests
    {
        private KycAggregationController _kycAggregationController;
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private FileCache _fileCache;
        private Mock<ILogger<KycAggregationController>> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            // Initialize mocks for dependencies
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockLogger = new Mock<ILogger<KycAggregationController>>();
      
            //Create an instance of KycAggregationController with the mocked dependencies
            _kycAggregationController = new KycAggregationController(
                _mockHttpClientFactory.Object,
                _fileCache,
                _mockLogger.Object
            );
        }

        [TestMethod]
        public async Task GetCustomerPersonalDetailsResponse_ShouldReturnSuccess_WhenApiCallIsSuccessful()
        {
            // Arrange
            var ssn = "19951212-3456";  //Used the Ssn provided to us
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK, //Simulate a successful API response
                    Content = new StringContent("{\"success\":true}")
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);

            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            // Act
            var personalDetailsResponse = await _kycAggregationController.GetCustomerPersonalDetailsResponse(ssn);

            // Assert
            Assert.IsTrue(personalDetailsResponse.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task GetCustomerPersonalDetailsResponse_ShouldReturnFailure_WhenApiCallFails()
        {
            // Arrange
            var ssn = "19951212-3456";  //Used the ssn provided to us
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest //Simulate a failed API response
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);

            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            // Act
            var personalDetailsResponse = await _kycAggregationController.GetCustomerPersonalDetailsResponse(ssn);

            // Assert
            Assert.IsFalse(personalDetailsResponse.IsSuccessStatusCode);
        }
    }
}
