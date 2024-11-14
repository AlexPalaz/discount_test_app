using DiscountApp.Services;
using Microsoft.AspNetCore.SignalR;

namespace DiscountApp.Hubs
{
    public class DiscountHub(IDiscountService discountService) : Hub
    {
        private readonly IDiscountService _discountService = discountService;

        public async Task<List<string>> GenerateCodes(ushort count, byte length)
        {
            return count > 2000
                ? throw new ArgumentException("Cannot generate more than 2000 codes at once.", nameof(count))
                : await _discountService.GenerateUniqueCodes(count, length);
        }

        public async Task<bool> UseCode(string code)
        {
            return await _discountService.UseCode(code);
        }
    }
}
