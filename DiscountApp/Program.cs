using DiscountApp.Hubs;
using DiscountApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<DiscountContext>(options =>
    options.UseSqlite(builder.Configuration.GetValue<string>("ConnectionStrings:DiscountDb")));
builder.Services.AddSignalR();

builder.Services.AddScoped<IDiscountService, DiscountService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<DiscountHub>("/discountHub");

app.Run();
