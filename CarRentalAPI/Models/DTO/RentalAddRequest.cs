namespace CarRentalAPI.Models.DTO
{
    public class RentalAddRequest
    {
        public int UserId { get; set; }
        public int VehicleId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
