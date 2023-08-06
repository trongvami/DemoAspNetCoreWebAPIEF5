using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Data;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis;
using WebApplication1.Persistent;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//builder.Services.AddMvc(options => {
//    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
//    .Build();
//    options.Filters.Add(new AuthorizeFilter(policy));
//}).AddXmlSerializerFormatters();
builder.Services.AddMvc();
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
builder.Services.AddDbContext<DbNet6Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbNet6Connect")));
builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 5000
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
{
    option.Password.RequiredLength = 9;
    option.Password.RequiredUniqueChars = 3;
    option.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<DbNet6Context>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(optionsd => {
    optionsd.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    optionsd.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    optionsd.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
})
        .AddCookie(option =>
        {
            option.Cookie.Name = "AccessToken";
            option.ExpireTimeSpan = TimeSpan.FromMinutes(20); // expire session
            option.SlidingExpiration = true;
            option.LoginPath = "/Admin/AdminAccount/Login";
            option.AccessDeniedPath = "/Admin/AdminAccount/AccessDenied";
        }).AddGoogle(option => {
            option.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            option.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            option.CallbackPath = "/signin-google";
        });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DeleteCreateRolePolicy", policy => policy.RequireClaim("Delete Role", "true").RequireClaim("Create Role", "true"));
    options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role", "true"));
    options.AddPolicy("CreateRolePolicy", policy => policy.RequireClaim("Create Role", "true"));
    options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true"));
    //options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // life session (example 20 minute)
    options.Cookie.IsEssential = true; // make sure your cookie session always be use
});
//builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = builder.Configuration["RedisCacheUrl"]; });
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseNotyf();

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=AdminHome}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
