using System.ComponentModel.DataAnnotations;

namespace Book_Rental.DTOs.Requests
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }
}
