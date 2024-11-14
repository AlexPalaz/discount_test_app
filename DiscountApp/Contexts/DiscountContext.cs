using DiscountApp.Models;
using Microsoft.EntityFrameworkCore;

public class DiscountContext : DbContext
{
    public DbSet<DiscountCode> DiscountCodes { get; set; }

    public DiscountContext(DbContextOptions<DiscountContext> options) : base(options) { }
}
