using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models.Identity
{
    public class UserUpdateDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
