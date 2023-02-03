using CarRentalAPI.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CarRentalAPI.Data
{
    public static class DbInitializer // SeedIdentity
    {
        public static async Task SeedUsers(IApplicationBuilder app) // Initialize
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                using (var context = new AppDbContext(
                scope.ServiceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
                {
                    var adminUser = await EnsureUser(serviceProvider, "TestUser", "Test123123!");
                    var testUser = await EnsureUser(serviceProvider, "TestUser2", "Test123123!");
                }
            }
        }

        //public static async Task SeedUsers2(this IServiceProvider serviceProvider)
        //{
        //    using (var scope = app.ApplicationServices.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        //        Console.WriteLine("Bonjur");
        //        // Use the userManager instance to perform operations on the users in your application.
        //        // ...
        //    }
        //}

        public static async Task SeedUsers3(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                Console.WriteLine("Bonjur 3");
                var users = await userManager.Users.ToListAsync();

                users.ForEach(x => Console.WriteLine(x.UserName));
                // Use the userManager instance to perform operations on the users in your application.
                // ...
            }
        }

        private static async Task<int> EnsureUser(
            IServiceProvider serviceProvider,
            string userName, string initPw) // Password
        {
            var userManager = serviceProvider.GetService<UserManager<AppUser>>();

            var user = await userManager.FindByNameAsync(userName);

            // Creating new user from scratch
            if (user == null)
            {
                user = new AppUser
                {
                    UserName = userName,
                    Email = userName+"@gmail.com",
                    EmailConfirmed = true,
                    FirstName = userName,
                    LastName = userName,
                    Birthday = new(1999, 06, 08)
                };

                var result = await userManager.CreateAsync(user, initPw);
            }

            if (user == null)
                throw new Exception("User did not get created. Password policy problem?");

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(
            IServiceProvider serviceProvider, string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            IdentityResult ir;

            if (await roleManager.RoleExistsAsync(role) == false)
            {
                ir = await roleManager.CreateAsync(new(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user is null)
                throw new Exception("User not existing");

            // Assigning role to user(add specific user to role)
            ir = await userManager.AddToRoleAsync(user, role);

            return ir;
        }

    }


}
