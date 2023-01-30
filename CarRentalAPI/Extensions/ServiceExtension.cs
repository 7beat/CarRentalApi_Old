using AutoMapper;
using CarRentalAPI.Data;
using CarRentalAPI.Models.Identity;
using CarRentalAPI.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("Jwt"); //JwtConfig
            var secretKey = jwtConfig["Key"]; //Secret
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["Issuer"], //ValidIssuer
                    ValidAudience = jwtConfig["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        //public static void ConfigureMapping(this IServiceCollection services)
        //{
        //    services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        //    var mapperConfig = new MapperConfiguration(map =>
        //    {
        //        map.AddProfile<UsersProfile>();
        //        map.AddProfile<VehiclesProfile>();
        //        //map.AddProfile<UserMappingProfile>();
        //    });
        //    services.AddSingleton(mapperConfig.CreateMapper());
        //}

    }
}
