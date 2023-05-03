using Microsoft.EntityFrameworkCore;
using ShoeApp.Models.Repositories;
using Context = ShoeApp.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Context.AppContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("AppContext"));
});

builder.Services.AddScoped<IShoeRepository, ShoeRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// umožňuje přístup do složky wwwroot z prohlížeče
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "shoe_list",
    pattern: "seznam-bot",
    defaults: new { controller = "Shoe", action = "Index" }
    );

// obecná routa, která zachytí veškeré požadavky a bude routovat ve formátu /{Controller}/{Akce} - /Movie/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();