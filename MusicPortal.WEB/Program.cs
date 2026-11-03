using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MusicPortal.BLL.Interfaces;
using MusicPortal.BLL.Services;
using MusicPortal.DAL.EF;
using MusicPortal.DAL.Interfaces;
using MusicPortal.DAL.Repositories;
using MusicPortal.WEB;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource));
    });
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// SQLite Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=musicportal.db"));

// Scoped Repositories & Services (Unit of Work + Low Coupling)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IAdminService, AdminService>();

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });

var app = builder.Build();

// Ensure DB Created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// Localization Setup
var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("uk") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("uk"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
