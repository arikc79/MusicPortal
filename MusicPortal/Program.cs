using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicPortal.Data;
using MusicPortal.Models;
using MusicPortal.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ----------------------  Localization ----------------------
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[]
{
    new CultureInfo("uk-UA"),
    new CultureInfo("en-US"),
    new CultureInfo("de-DE")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("uk-UA"); // 🇺🇦 українська — дефолт
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    //  Cookie має найвищий пріоритет
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});

// ----------------------  Database ----------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ----------------------  Dependency Injection ----------------------
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IGenreService, GenreService>();

// ----------------------  Authentication ----------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
});

// ----------------------  MVC + Filters + Localization ----------------------
builder.Services.AddControllersWithViews(options =>
{
})
.AddViewLocalization()
.AddDataAnnotationsLocalization();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();


var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);


// ----------------------  Middleware Pipeline ----------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCookiePolicy();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// ----------------------  Default Routing ----------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ----------------------  Seed Admin User ----------------------
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    if (!context.Users.Any(u => u.UserName == "admin"))
    {
        var hasher = new PasswordHasher<User>();
        var admin = new User
        {
            UserName = "admin",
            Email = "admin@portal.local",
            Role = UserRole.Admin,
            IsActive = true
        };

        admin.PasswordHash = hasher.HashPassword(admin, "admin");
        context.Users.Add(admin);
        context.SaveChanges();

        Console.WriteLine("✅ Admin user created: login = admin, password = admin");
    }
}

app.Run();
