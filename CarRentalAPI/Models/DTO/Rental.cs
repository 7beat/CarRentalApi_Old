using CarRentalAPI.Models.Identity;

namespace CarRentalAPI.Models.DTO
{
    public class Rental
    {
        public int Id { get; set; }
        //public AppUser User { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
