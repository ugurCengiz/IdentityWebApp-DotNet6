using System.Security.Claims;
using IdentityUyelikSistemi_DotNet6;
using IdentityUyelikSistemi_DotNet6.ClaimProvider;
using IdentityUyelikSistemi_DotNet6.CustomValidation;
using IdentityUyelikSistemi_DotNet6.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//Servicesleri buraya yazıyoruz

builder.Services.AddTransient<IAuthorizationHandler, ExpireDateExchangeHandler>();

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("AnkaraPolicy", policy =>
    {
        policy.RequireClaim("city", "ankara");
    });

    opts.AddPolicy("ViolancePolicy", policy =>
    {
        policy.RequireClaim("violance");
    });


    opts.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExpireDateExchangeRequirement());
    });

});

builder.Services.AddAuthentication().AddFacebook(opts =>
{
    opts.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    opts.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];

});

builder.Services.AddIdentity<AppUser, AppRole>(opts =>
{
    opts.User.RequireUniqueEmail = true;
    opts.User.AllowedUserNameCharacters = "abcçdefgğðhýıijklmnoöpqrsşþtuüvwxyzABCÇDEFGÐHIÝJKLMNOÖPQRSÞTUÜVWXYZ0123456789-._";
    opts.Password.RequiredLength = 4;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;

}).AddPasswordValidator<CustomPasswordValidator>()
    .AddUserValidator<CustomUserValidator>()
    .AddErrorDescriber<CustomIdentityErrorDescriber>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();
CookieBuilder cookieBuilder = new CookieBuilder();

cookieBuilder.Name = "MyBlog";
cookieBuilder.HttpOnly = false;
cookieBuilder.SameSite = SameSiteMode.Lax;
cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = new PathString("/Home/Login");
    opts.LogoutPath = new PathString("/Member/LogOut");
    opts.Cookie = cookieBuilder;
    opts.SlidingExpiration = true;
    opts.ExpireTimeSpan = TimeSpan.FromDays(60);
    opts.AccessDeniedPath = new PathString("/Member/AccessDenied");
});
builder.Services.AddScoped<IClaimsTransformation, ClaimProvider>();

builder.Services.AddMvc();


var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStatusCodePages();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
