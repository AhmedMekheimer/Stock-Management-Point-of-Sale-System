using CoreLayer;
using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Utility;

namespace Presentation_Layer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            // DB Configurations
            builder.Services.AddDbContext<ApplicationDbContext>(
                option => option.UseSqlServer("Data Source=.;Initial Catalog=Stock and POS System2; Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;")
            );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Authentication
            builder.Services.ConfigureApplicationCookie(
                options =>
                {
                    options.LoginPath = "/Identity/Account/SignIn";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                }
                );

            // Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy($"{SD.Workers}", policy =>
                      policy.RequireRole($"{SD.StockManager}", $"{SD.BranchManager}", $"{SD.Cashier}"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

    

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Dashboard}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();


            using (var scope = app.Services.CreateScope())
            {

                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                // use DbInitializer
                dbInitializer.Init();
            }

            app.Run();
        }
    }
}
