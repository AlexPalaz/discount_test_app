namespace DiscountApp.Services
{
    public interface IDiscountService
    {
        Task<bool> UseCode(string code);
        Task<List<string>> GenerateUniqueCodes(ushort count, byte length);
    }
}
