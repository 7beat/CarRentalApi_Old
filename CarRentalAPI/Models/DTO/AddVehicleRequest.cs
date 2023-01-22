using CarRentalAPI.Models.Domain;

namespace CarRentalAPI.Models.DTO
{
    public class AddVehicleRequest
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Color { get; set; }
        public DateOnly YearOfProduction { get; set; }
    }
}
