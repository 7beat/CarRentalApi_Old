using CarRentalAPI.Models.Domain;

namespace CarRentalAPI.Models.DTO
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public Color Color { get; set; } //By default it will use System.Drawing
        public DateOnly YearOfProduction { get; set; }

        //public int? UserId { get; set; }
    }
}
