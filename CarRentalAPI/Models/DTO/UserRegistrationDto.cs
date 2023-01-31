using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models.DTO
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public DateOnly BirthDay { get; set; }
    }
}
