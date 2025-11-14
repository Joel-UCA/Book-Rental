using Book_Rental.Constants;
using System.ComponentModel.DataAnnotations;

namespace Book_Rental.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = "";

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = Roles.User; // Default role is "User"

        // Rental tracking is handled through RentalHistory table
    }
}
