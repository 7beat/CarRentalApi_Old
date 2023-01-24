using CarRentalAPI.Repositories;
using CarRentalAPI.Validators;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models.DTO
{
    public class AddUserRequest
    {
        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateOnly BirthDay { get; set; }
    }
}
