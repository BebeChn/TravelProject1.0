using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using TravelProject1._0.Models;

using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TravelProject1._0
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<TravelProjectAzureContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("TravelProject"));
            });

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
            {
                option.LoginPath = "/User/Login";//
                option.AccessDeniedPath = "/Home/NoAuthority";
            });
            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            //    options.LoginPath = "/Identity/Account/Login";
            //    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            //    options.SlidingExpiration = true;
            //});

            builder.Services.AddSession();
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

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}