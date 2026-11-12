using MefaOzelEgitim.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using MefaOzelEgitim.DataAccess.Repositories;
using MefaOzelEgitim.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using MefaOzelEgitim.DataAccess.Seed;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MefaDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options =>
{
   // * sifre kurallari
   options.Password.RequireDigit = true;
   options.Password.RequiredLength = 6;
   options.Password.RequireNonAlphanumeric = false;
   options.Password.RequireUppercase = false;
   options.Password.RequireLowercase = false; 
})
.AddEntityFrameworkStores<MefaDbContext>()
.AddDefaultTokenProviders();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// * admin area
app.MapControllerRoute(
    name:"areas",
    pattern:"{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbInitializer.SeedAsync(services);
    }
    catch(Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex,"Veritabanı seed işlemi sırasında bir hata oluştu.");
    }
}

app.Run();
