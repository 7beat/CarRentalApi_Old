using CarRentalAPI.Data;
using CarRentalAPI.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace CarRentalAPI.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/>
    /// </summary>
    public static class AppBuilderExtension
    {
        /// <summary>
        /// Seeds related IdentityUsers of type <see cref="Models.Identity.AppUser"/> and Roles in DataBase
        /// </summary>
        /// <remarks>Default Accounts: Admin, User. Password = Password123!</remarks>
        /// <param name="app">An instance of <see cref="IApplicationBuilder"/>.</param>
        /// <returns></returns>
        public static async Task SeedIdentityDb(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                using (var context = new AppDbContext(
                scope.ServiceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
                {
                    if (!context.Database.GetPendingMigrations().Any())
                        context.Database.Migrate();

                    var adminUser = await EnsureUser(serviceProvider, "Admin", "Password123!");
                    await EnsureRole(serviceProvider, adminUser, "Admin");

                    var normalUser = await EnsureUser(serviceProvider, "User", "Password123!");
                    await EnsureRole(serviceProvider, normalUser, "User");
                }
            }
        }

        private static async Task<int> EnsureUser(
            IServiceProvider serviceProvider,
            string userName, string initPw)
        {
            var userManager = serviceProvider.GetService<UserManager<AppUser>>();

            var user = await userManager.FindByNameAsync(userName);

            // Creating new user
            if (user is null)
            {
                user = new AppUser
                {
                    UserName = userName,
                    Email = userName + "@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "John",
                    LastName = "Doe",
                    Birthday = new(1999, 06, 08)
                };

                var result = await userManager.CreateAsync(user, initPw);
            }

            if (user is null)
                throw new Exception("User did not get created. Password policy problem?");

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(
            IServiceProvider serviceProvider, int uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole<int>>>();

            IdentityResult ir;

            if (!await roleManager.RoleExistsAsync(role))
            {
                ir = await roleManager.CreateAsync(new(role));
            }

            var userManager = serviceProvider.GetService<UserManager<AppUser>>();

            var user = await userManager.FindByIdAsync(uid.ToString());

            if (user is null)
                throw new Exception("User not existing");

            // Assigning role to user(add specific user to role)
            ir = await userManager.AddToRoleAsync(user, role);

            return ir;
        }
    }
}