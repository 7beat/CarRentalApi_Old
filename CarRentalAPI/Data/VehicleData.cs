using CarRentalAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalAPI.Data
{
    public class VehicleData : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasData(
                new Vehicle
                {
                    Id = 1,
                    Brand = "Ford",
                    Model = "Mondeo",
                    YearOfProduction = new(2017, 2, 15),
                    ColorId = 1,
                },
                new Vehicle
                {
                    Id = 2,
                    Brand = "Mercedes",
                    Model = "GLC",
                    YearOfProduction = new(2019, 6, 8),
                    ColorId = 1,
                },
                new Vehicle
                {
                    Id = 3,
                    Brand = "Honda",
                    Model = "Civic",
                    YearOfProduction = new(2008, 8, 21),
                    ColorId = 2,
                }
                );
        }
    }
}
