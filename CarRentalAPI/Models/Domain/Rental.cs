namespace CarRentalAPI.Models.Domain
{
    public class Rental
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

    }
}
