using CarRentalAPI.Data;
using CarRentalAPI.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace CarRentalAPI.Extensions
{
    public static class ServiceExtension
    {

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<AppUser, IdentityRole<int>>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireUppercase = false;

                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
