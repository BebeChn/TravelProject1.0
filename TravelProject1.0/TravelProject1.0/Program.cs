using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using TravelProject1._0.Services;
using TravelProject1._0.Hubs;
using Microsoft.AspNetCore.Authorization;

namespace TravelProject1._0
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TravelProjectAzureContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("TravelProject"));
            });

            builder.Services.AddSignalR();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddTransient<IUserIdentityService, UserIdentityService>();
            builder.Services.AddTransient<IUserSearchService, UserSearchService>();
            builder.Services.AddTransient<IProductSearchService, ProductSearchService>();

            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(opt => { 
                opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
            {
                option.LoginPath = "/User/Login";
                option.AccessDeniedPath = "/Home/NoAuthority";
            }).AddCookie("Admin", option =>
            {
                option.LoginPath = "/Admin/Manage/Login";
                option.AccessDeniedPath = "/Home/NoAuthority";
            });
            builder.Services.AddAuthorization(opt =>
            {
                var adminPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes("Admin").Build();
                var userPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme).Build();
                opt.AddPolicy("admin", adminPolicy);
                opt.AddPolicy("user", userPolicy);
                opt.DefaultPolicy = userPolicy;

            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/User/Login";
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.None;
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapHub<UserHub>("/userHub");

            app.Run();
        }
    }
}