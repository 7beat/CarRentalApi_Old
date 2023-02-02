using CarRentalAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalAPI.Data
{
    public class ColorData : IEntityTypeConfiguration<Models.Domain.Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.HasData(
                new Color
                {
                    Id = 1,
                    Name = "Black",
                },
                new Color
                {
                    Id = 2,
                    Name= "White",
                }
                );
        }
    }
}
