using DiscountApp.Models;
using DiscountApp.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text;

public class DiscountService : IDiscountService
{
    private readonly DiscountContext _context;
    private static Random random = new();

    public DiscountService(DiscountContext context)
    {
        _context = context;
    }

    public async Task<bool> UseCode(string code)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var discountCode = await _context.DiscountCodes.FirstOrDefaultAsync(c => c.Code == code);

            if (discountCode is null)
            {
                throw new HubException("The provided code does not exist.");
            }

            if (discountCode.Consumed)
            {
                return false;
            }

            discountCode.Consumed = true;
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new HubException("The discount code has already been used by another operation.");
        }
        catch (Exception ex)
        {
            throw new HubException(ex.Message);
        }
    }

    public async Task<List<string>> GenerateUniqueCodes(ushort count, byte length)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var generatedDiscountCodes = new List<DiscountCode>();

            for (var i = 0; i < count; i++)
            {
                var code = GenerateCode(length);

                while (await _context.DiscountCodes.AnyAsync(c => c.Code == code))
                {
                    code = GenerateCode(length);
                }

                generatedDiscountCodes.Add(new DiscountCode {
                    Code = code,
                    Consumed = false
                });
            }

            await _context.AddRangeAsync(generatedDiscountCodes);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return generatedDiscountCodes.Select((code) => code.Code).ToList();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new HubException(ex.Message);
        }
    }

    private static string GenerateCode(byte length)
    {
        if (length is not 7 and not 8)
        {
            throw new HubException("The length of the code must be 7 or 8 characters.");
        }

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var output = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            output.Append(chars[random.Next(chars.Length)]);
        }

        return output.ToString();
    }
}
