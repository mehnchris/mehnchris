using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using RepVault.Data;
using RepVault.Models;
using RepVault.RepVaultServices;

var builder = WebApplication.CreateBuilder(args);

// ✅ Use one consistent connection string key
var connectionString = builder.Configuration.GetConnectionString("RepVaultConnection")
    ?? throw new InvalidOperationException("Connection string 'RepVaultConnection' not found.");

// ✅ Register DbContext with retry logic
builder.Services.AddDbContext<RepVaultDbContext>(options =>
    options.UseSqlServer(connectionString,
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    )
);

// ✅ Identity setup with email confirmation and unique emails
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<RepVaultDbContext>();

// ✅ Email service using SendGrid
builder.Services.AddTransient<IEmailSender, EmailSender>();

// ✅ MVC & Razor Pages
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// ✅ Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Enforce HTTPS
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Identity login
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
