using Microsoft.AspNetCore.Identity;
using NToastNotify;
using ToDoListMVC.Data.Context;
using ToDoListMVC.Data.Extensions;
using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.LoadDataLayerExtension(builder.Configuration);
builder.Services.LoadServiceLayerExtension();
builder.Services.AddSession();
// Add services to the container.

builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        PositionClass = ToastPositions.TopRight,
        TimeOut = 1000,
        ProgressBar = true,
        
    })
    .AddRazorRuntimeCompilation();

builder.Services.AddIdentity<AppUser, IdentityRole<int>>(opt =>
{
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = new PathString("/Auth/Login");
    config.LogoutPath = new PathString("/Auth/Logout");
    config.Cookie = new CookieBuilder
    {
        Name = "ToDoListMVC",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        SecurePolicy = CookieSecurePolicy.SameAsRequest
    };
    config.SlidingExpiration = true;
    config.ExpireTimeSpan = TimeSpan.FromDays(1);
    config.AccessDeniedPath = new PathString("/Auth/AccessDenied");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseNToastNotify();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "User",
    pattern: "{controller=User}/{action=Index}/{userId?}");

app.Run();
