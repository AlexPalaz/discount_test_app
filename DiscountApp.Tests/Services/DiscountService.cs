using DiscountApp.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DiscountApp.Tests
{
    public class DiscountServiceTests
    {
        private DiscountContext _context;
        private DiscountService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DiscountContext>()
                .UseSqlite("Filename=:memory:")
                .Options;

            _context = new DiscountContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            _service = new DiscountService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
        }

        [Test]
        public async Task UseCode_ValidCode_ReturnsTrue()
        {
            var discountCode = new DiscountCode { Code = "VALID123", Consumed = false };
            _context.DiscountCodes.Add(discountCode);
            await _context.SaveChangesAsync();

            var result = await _service.UseCode("VALID123");

            Assert.IsTrue(result);
            Assert.IsTrue(discountCode.Consumed);
        }

        [Test]
        public void UseCode_NonExistentCode_ThrowsException()
        {
            var ex = Assert.ThrowsAsync<HubException>(() => _service.UseCode("NONEXISTENT"));
            Assert.That(ex.Message, Is.EqualTo("The provided code does not exist."));
        }

        [Test]
        public async Task UseCode_AlreadyConsumedCode_ReturnsFalse()
        {
            var discountCode = new DiscountCode { Code = "USED123", Consumed = true };
            _context.DiscountCodes.Add(discountCode);
            await _context.SaveChangesAsync();

            var result = await _service.UseCode("USED123");

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GenerateUniqueCodes_ValidParameters_ReturnsCodes()
        {
            var codes = await _service.GenerateUniqueCodes(3, 7);

            Assert.That(codes.Count, Is.EqualTo(3));
            Assert.IsTrue(codes.All(code => code.Length == 7));
        }

        [Test]
        public void GenerateUniqueCodes_CodeLengthInvalid_ThrowsException()
        {
            var ex = Assert.ThrowsAsync<HubException>(() => _service.GenerateUniqueCodes(3, 6));
            Assert.That(ex.Message, Is.EqualTo("The length of the code must be 7 or 8 characters."));
        }

        [Test]
        public async Task GenerateUniqueCodes_DuplicateCodeRetries()
        {
            _context.DiscountCodes.Add(new DiscountCode { Code = "DUPLICATE1", Consumed = false });
            await _context.SaveChangesAsync();

            var codes = await _service.GenerateUniqueCodes(3, 7);

            Assert.That(codes.Count, Is.EqualTo(3));
            Assert.IsTrue(codes.Distinct().Count() == codes.Count);
        }
    }
}
