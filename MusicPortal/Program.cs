using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicPortal.Data;
using MusicPortal.Models;
using MusicPortal.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------------------- DB ----------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------- Services ----------------------
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IGenreService, GenreService>();

// ---------------------- Cookies ----------------------
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

// ---------------------- MVC + Validation ----------------------
builder.Services
    .AddControllersWithViews()
    .AddViewOptions(o => o.HtmlHelperOptions.ClientValidationEnabled = true);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// ---------------------- Pipeline ----------------------
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ---------------------- ✅ Seed admin user ----------------------
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated(); // створює БД, якщо немає

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
