using CarRentalAPI.Models.Domain;
using CarRentalAPI.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        public DbSet<Color> Colors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>().Property(x => x.AppUserId).HasColumnName("UserId");
            //modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            //modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.ApplyConfiguration(new ColorData());
            modelBuilder.ApplyConfiguration(new VehicleData());
            modelBuilder.ApplyConfiguration(new RoleData());
        }
    }
}
