using CarRentalAPI.Models.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalAPI.Models.Domain
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateOnly YearOfProduction { get; set; }

        public Color Color { get; set; }
        public int ColorId { get; set; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [Column("UserId")]
        public int? UserId { get; set; }
    }
}
