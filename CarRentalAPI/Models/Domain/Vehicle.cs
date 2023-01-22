using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalAPI.Models.Domain
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public Color Color { get; set; }
        public DateOnly YearOfProduction { get; set; }

        public int? UserId { get; set; }
    }
}
