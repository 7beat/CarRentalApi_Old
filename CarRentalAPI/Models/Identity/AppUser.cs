using CarRentalAPI.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace CarRentalAPI.Models.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        //public DateOnly? Birthday { get; set; }
        //public ICollection<Vehicle> Vehicles { get; set; }
    }
}
