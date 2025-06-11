using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using RepVault.Data;
using RepVault.Models;
using RepVault.RepVaultServices;

var builder = WebApplication.CreateBuilder(args);

// Configure the database connection (ensure RepVaultConnection exists in Azure Configuration)
var connectionString = builder.Configuration.GetConnectionString("RepVaultConnection")
    ?? throw new InvalidOperationException("Connection string 'RepVaultConnection' not found.");

builder.Services.AddDbContext<RepVaultDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<RepVaultDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("RepVaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    )
);


// Identity setup with email confirmation and unique emails
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<RepVaultDbContext>();

// Email service (ensure SendGrid is configured in production)
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Dev exception filter
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Middleware setup
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Enforce HTTPS in production
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Ensure this is here so styles/js work after deployment

app.UseRouting();
app.UseAuthentication(); // ✅ Needed for Identity
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
