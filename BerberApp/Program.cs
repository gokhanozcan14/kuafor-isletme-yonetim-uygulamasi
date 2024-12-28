using BerberApp.Data;
using BerberApp.Filters;
using BerberApp.Models;
using BerberApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<RequireLoginFilter>();
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<StaffService>();
builder.Services.AddScoped<PackageService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RequireLoginFilter>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

var app = builder.Build();

// Ensure the admin user exists
EnsureAdminUserExists(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

// Use session middleware
app.UseSession();

app.MapControllerRoute(
    name: "staff_edit",
    pattern: "admin/staff/edit/{id?}",
    defaults: new { controller = "Admin", action = "EditStaff" });

app.MapControllerRoute(
    name: "staff_delete",
    pattern: "admin/staff/delete/{id?}",
    defaults: new { controller = "Admin", action = "DeleteStaff" });

app.MapControllerRoute(
    name: "package_edit",
    pattern: "admin/package/edit/{id?}",
    defaults: new { controller = "Admin", action = "EditPackage" });

app.MapControllerRoute(
    name: "package_delete",
    pattern: "admin/package/delete/{id?}",
    defaults: new { controller = "Admin", action = "DeletePackage" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void EnsureAdminUserExists(IServiceProvider services)
{
    using (var scope = services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (!context.Users.Any(u => u.Role == "Admin"))
        {
            var adminUser = new User
            {
                Username = "G201210052@sakarya.edu.tr",
                Password = "sau",
                Role = "Admin"
            };
            context.Users.Add(adminUser);
            context.SaveChanges();
        }
    }
}