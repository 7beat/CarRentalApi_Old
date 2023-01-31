using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models.Identity
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; init; }

        //Optional
        //public string Username { get; init; }
    }
}
