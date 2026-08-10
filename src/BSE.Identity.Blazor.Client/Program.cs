using Azure.Identity;
using BSE.Identity.Blazor.Client.Areas.Identity;
using BSE.Identity.Blazor.Client.Data;
using BSE.Identity.Blazor.Client.Extensions;
using BSE.Identity.Blazor.Client.Models;
using BSE.Identity.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using MySqlConnector;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    using var x509Store = new X509Store(StoreLocation.LocalMachine);
    x509Store.Open(OpenFlags.ReadOnly);
    var x509Certificate = x509Store.Certificates
    .Find(
        X509FindType.FindByThumbprint,
        builder.Configuration["KeyVault:AzureADCertThumbprint"],
        validOnly: false)
    .OfType<X509Certificate2>()
    .Single();

    builder.Configuration.AddAzureKeyVault(
            new Uri($"https://{builder.Configuration["KeyVault:Name"]}.vault.azure.net/"),
            new ClientCertificateCredential(
                builder.Configuration["KeyVault:AzureADDirectoryId"],
                builder.Configuration["KeyVault:AzureADApplicationId"],
                x509Certificate));
}

var connectionStringBuilder = new MySqlConnectionStringBuilder
{
    Server = builder.Configuration["identity:backend:server"],
    Database = builder.Configuration["identity:backend:database"],
    UserID = builder.Configuration["identity:backend:userid"],
    Password = builder.Configuration["identity:backend:password"]
};

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseMySql(connectionStringBuilder.ConnectionString, ServerVersion.AutoDetect(connectionStringBuilder.ConnectionString))
    );
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Configure AddFluentUIComponents() service collection extension
builder.Services.AddHttpClient();

builder.Services.AddFluentUIComponents();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

//var supportedCultures = new[] { "en-US", "de-de" };
//var localizationOptions = new RequestLocalizationOptions()
//    .SetDefaultCulture(supportedCultures[0])
//    .AddSupportedCultures(supportedCultures)
//    .AddSupportedUICultures(supportedCultures);

//app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
