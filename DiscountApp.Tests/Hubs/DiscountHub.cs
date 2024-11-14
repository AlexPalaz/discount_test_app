using DiscountApp.Hubs;
using DiscountApp.Services;
using Microsoft.AspNetCore.SignalR;
using NSubstitute;

namespace DiscountApp.Tests
{
    public class DiscountHubTests
    {
        private DiscountHub _hub;
        private IDiscountService _discountServiceMock;

        [SetUp]
        public void Setup()
        {
            _discountServiceMock = Substitute.For<IDiscountService>();
            _hub = new DiscountHub(_discountServiceMock);
        }

        [TearDown]
        public void TearDown()
        {
            _hub.Dispose();
        }

        [Test]
        public async Task GenerateCodes_ValidRequest_ReturnsCodes()
        {
            var count = (ushort)3;
            var length = (byte)7;
            var generatedCodes = new List<string> { "CODE123", "CODE456", "CODE789" };
            _discountServiceMock.GenerateUniqueCodes(count, length).Returns(generatedCodes);

            var result = await _hub.GenerateCodes(count, length);

            Assert.That(result, Is.EqualTo(generatedCodes));
            await _discountServiceMock.Received(1).GenerateUniqueCodes(count, length);
        }

        [Test]
        public void GenerateCodes_ExceedsLimit_ThrowsException()
        {
            var count = (ushort)2001;
            var length = (byte)7;

            var ex = Assert.ThrowsAsync<HubException>(() => _hub.GenerateCodes(count, length));
            Assert.That(ex.Message, Is.EqualTo("Cannot generate more than 2000 codes at once."));
        }

        [Test]
        public async Task UseCode_ValidCode_ReturnsTrue()
        {
            var code = "VALID123";
            _discountServiceMock.UseCode(code).Returns(true);

            var result = await _hub.UseCode(code);

            Assert.IsTrue(result);
            await _discountServiceMock.Received(1).UseCode(code);
        }

        [Test]
        public async Task UseCode_InvalidCode_ReturnsFalse()
        {
            var code = "INVALID123";
            _discountServiceMock.UseCode(code).Returns(false);

            var result = await _hub.UseCode(code);

            Assert.IsFalse(result);
            await _discountServiceMock.Received(1).UseCode(code);
        }
    }
}
