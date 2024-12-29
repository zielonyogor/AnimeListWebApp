using Oracle.ManagedDataAccess.Client;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Microsoft.AspNetCore.Identity;
using Application.Models;
using Microsoft.Extensions.DependencyInjection;

async Task SeedRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<Account>>();

    // Adds roles, delete this later probably
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole<int>("Admin"));
    }

    if (!await roleManager.RoleExistsAsync("Moderator"))
    {
        await roleManager.CreateAsync(new IdentityRole<int>("Moderator"));
    }

    // Delete this later
    var adminUser = await userManager.FindByNameAsync("superguy44");
    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
    var modUser = await userManager.FindByNameAsync("asdfgh");
    if (modUser != null && !await userManager.IsInRoleAsync(modUser, "Moderator"))
    {
        await userManager.AddToRoleAsync(modUser, "Moderator");
    }
}


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ModelContext>(options =>
   options.UseOracle(builder.Configuration.GetConnectionString("OracleDB")));

builder.Services.AddIdentity<Account, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ModelContext>()
    .AddDefaultTokenProviders();

// For returning StatusCodes for API and Redirecting for Views
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"message\": \"Access Denied\"}");
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        }
    };

    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"message\": \"Authentication required\"}");
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddHttpClient("Application", c =>
{
    c.BaseAddress = new Uri("http://localhost:5074");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    await SeedRoles(scope.ServiceProvider);
}

app.Run();
