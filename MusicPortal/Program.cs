using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MusicPortal.Data;
using MusicPortal.Services;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// 1️⃣ Налаштовуємо DI (Services)
// -------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IGenreService, GenreService>();

// -------------------------
// 2️⃣ Cookie Authentication (оновлено)
// -------------------------
builder.Services.AddAuthentication(options =>
{
    // встановлюємо cookie-схему як основну
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax; // важливо
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
});

// -------------------------
// 3️⃣ MVC + клієнтська валідація
// -------------------------
builder.Services
    .AddControllersWithViews()
    .AddViewOptions(options =>
    {
        options.HtmlHelperOptions.ClientValidationEnabled = true;
    });

// -------------------------
// 4️⃣ Сесії
// -------------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// -------------------------
// 5️⃣ Побудова та pipeline
// -------------------------
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 🧱 важливо: кукі-політика перед авторизацією
app.UseCookiePolicy();

app.UseSession();
app.UseAuthentication();  // повинно йти ДО Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
