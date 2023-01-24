using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models.DTO
{
    public class UpdateUserRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateOnly BirthDay { get; set; }
    }
}
