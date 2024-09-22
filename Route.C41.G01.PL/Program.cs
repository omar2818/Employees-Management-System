using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.DAL.Data;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.Hepers;
using Route.C41.G01.PL.Services.EmailSender;
using Route.C41.G01.PL.Services.Settings;
using System;

namespace Route.C41.G01.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);
            #region Configure services
            webApplicationBuilder.Services.AddControllersWithViews();

            //services.AddScoped<ApplicationDbContext>();
            ////services.AddTransient<ApplicationDbContext>();
            ////services.AddSingleton<ApplicationDbContext>();

            //services.AddScoped<DbContextOptions<ApplicationDbContext>>();

            webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Scoped);

            webApplicationBuilder.Services.AddApplicationServices();
            webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));


            webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 5;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(5);

                //options.User.AllowedUserNameCharacters = "asdfgh987456321";
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            webApplicationBuilder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.AccessDeniedPath = "/Home/Error";
            });

            webApplicationBuilder.Services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = "Identity.Application";
            }).AddCookie("Hamda", options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.AccessDeniedPath = "/Home/Error";
            });
            webApplicationBuilder.Services.AddTransient<Services.EmailSender.IEmailSender, EmailSender>();

            webApplicationBuilder.Services.Configure<MailSettings>(webApplicationBuilder.Configuration.GetSection("MailSettings"));
            webApplicationBuilder.Services.AddTransient<IMailSettings, EmailSettings>();



            #endregion

            var app = webApplicationBuilder.Build();

            #region Configure Kestrel Middlewares
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion
            app.Run();
            //CreateHostBuilder(args).Build().Run();
        }
    }
}
