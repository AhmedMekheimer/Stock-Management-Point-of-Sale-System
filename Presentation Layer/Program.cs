using CoreLayer;
using CoreLayer.Models;
using InfrastructureLayer;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces;
using InfrastructureLayer.Services;
using InfrastructureLayer.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Utility;

namespace PresentationLayer
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
                option => option.UseSqlServer("Data Source=.;Initial Catalog=Stock and POS System; Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;")
            );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            // Authentication
            builder.Services.ConfigureApplicationCookie(
                options =>
                {
                    options.LoginPath = "/Identity/Account/Login";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                }
                );


            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ClothingItem.Add|ClothingItem.Edit", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim("Permission", "ClothingItem.Add") ||
                        context.User.HasClaim("Permission", "ClothingItem.Edit")
                    ));
            });

            //// Authorization
            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy($"{SD.Workers}", policy =>
            //          policy.RequireRole($"{SD.StockManager}", $"{SD.BranchManager}", $"{SD.Cashier}"));

            //    options.AddPolicy($"{SD.Managers}", policy =>
            //          policy.RequireRole($"{SD.StockManager}", $"{SD.BranchManager}", $"{SD.SuperAdmin}"));
            //});

            // In Startup.cs or Program.cs
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder => builder
                        .WithOrigins("https://yourfrontenddomain.com")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
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

            app.UseCors("AllowFrontend");

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Identity}/{controller=Account}/{action=Login}/{id?}")
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
