using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using RepVault.Data;
using RepVault.Models;
using RepVault.RepVaultServices;

var builder = WebApplication.CreateBuilder(args);

// ✅ Ensure correct connection string key
var connectionString = builder.Configuration.GetConnectionString("RepVaultConnection")
    ?? throw new InvalidOperationException("Connection string 'RepVaultConnection' not found.");

// ✅ Configure DbContext with retry logic
builder.Services.AddDbContext<RepVaultDbContext>(options =>
    options.UseSqlServer(connectionString,
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    )
);

// ✅ Identity setup with UI and email confirmation
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<RepVaultDbContext>()
.AddDefaultUI(); // <-- This line is required for /Account/Login to work
// AddDefaultTokenProviders(); // Optional if you're doing password reset, 2FA, etc.

// ✅ Email service via SendGrid
builder.Services.AddTransient<IEmailSender, EmailSender>();

// ✅ MVC, Razor Pages, Exception Filters
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
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ✅ Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ✅ Razor Pages for Identity UI
app.MapRazorPages();

app.Run();
