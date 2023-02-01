using CarRentalAPI.Models.Identity;

namespace CarRentalAPI.Models.Domain
{
    public class Rental
    {
        public int Id { get; set; }
        public AppUser User { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public int UserId { get; set; }
        public int VehicleId { get; set; }
    }
}
