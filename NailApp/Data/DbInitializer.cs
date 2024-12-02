using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NailApp.Models;

namespace NailApp.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();

                if(!context.Users.Any())
                {
                    var user = new ApplicationUser
                    {
                        UserName = "testuser@example.com",
                        Name = "Teste User",
                        Email = "testuser@example.com",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(user, "Test@1234");

                    var admin = new ApplicationUser
                    {
                        UserName = "admin@example.com",
                        Name = "Admin",
                        Email = "admin@example.com",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(admin, "Admin@1234");
                }

                if (await roleManager.RoleExistsAsync("Admin") == false)
                {
                    var role = new IdentityRole("Admin");
                    await roleManager.CreateAsync(role);
                }

                if (await roleManager.RoleExistsAsync("User") == false)
                {
                    var role = new IdentityRole("User");
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}
