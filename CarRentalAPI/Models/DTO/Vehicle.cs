using CarRentalAPI.Models.Domain;

namespace CarRentalAPI.Models.DTO
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public DateOnly YearOfProduction { get; set; }
    }
}
