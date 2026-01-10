using Area52.Data.Data;
using Area52.Data.Repositories;
using Area52.Domain.Interfaces;
using Area52.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register DatabaseContext
builder.Services.AddSingleton<DatabaseContext>();

// Register Repositories
builder.Services.AddSingleton<IBikeRepository, BikeRepository>();
builder.Services.AddSingleton<IBikeReservationRepository, BikeReservationRepository>();

// Register Services
builder.Services.AddSingleton<IBikePricingService, BikePricingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
