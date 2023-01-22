using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models.Domain
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, EmailAddress, MaxLength(30), MinLength(3)]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateOnly BirthDay { get; set; }


        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
