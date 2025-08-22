using CoreLayer;
using CoreLayer.Models;
using InfrastructureLayer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PresentationLayer.Utility
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _Context;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly UserManager<ApplicationUser> _UserManager;
        public DbInitializer(ApplicationDbContext context,
                            RoleManager<IdentityRole> roleManager,
                            UserManager<ApplicationUser> userManager)
        {
            _Context = context;
            _RoleManager = roleManager;
            _UserManager = userManager;
        }


        public void Init()
        {
            _Context.Database.EnsureCreated();

            if (_Context.Database.GetPendingMigrations().Any())
            {
                _Context.Database.Migrate();
            }

            if (!_Context.Users.Any() || !_Context.Roles.Any() || !_Context.Partners.Any())
            {
                _RoleManager.CreateAsync(new(SD.SuperAdmin)).GetAwaiter().GetResult();
                _RoleManager.CreateAsync(new(SD.StockManager)).GetAwaiter().GetResult();
                _RoleManager.CreateAsync(new(SD.BranchManager)).GetAwaiter().GetResult();
                _RoleManager.CreateAsync(new(SD.Cashier)).GetAwaiter().GetResult();

                var result = _UserManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "SuperAdmin",
                    Email = "Email@gmail.com",
                    EmailConfirmed = true,
                }, "SuperAdmin1!").GetAwaiter().GetResult();

                if (result.Succeeded)
                {
                    var Admin = SD.SuperAdmin == "Super Admin" ? "SuperAdmin" : null;
                    var superAdmin = _UserManager.FindByNameAsync(Admin).GetAwaiter().GetResult();

                    if (superAdmin is not null)
                    {
                        _UserManager.AddToRoleAsync(superAdmin, SD.SuperAdmin).GetAwaiter().GetResult();
                    }
                }

                _Context.Partners.Add(new Partner
                {
                    Name = "Retail",
                    partnerType = Partner.PartnerType.RetailCustomer
                });
               _Context.SaveChanges();
            }
        }
    }
}
