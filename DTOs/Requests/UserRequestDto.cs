using Book_Rental.Constants;
using System.ComponentModel.DataAnnotations;

namespace Book_Rental.DTOs.Requests
{
    public class UserRequestDto
    {
        [Required(ErrorMessage = ValidationMessages.FullNameRequired)]
        [StringLength(100, MinimumLength = 2, ErrorMessage = ValidationMessages.FullNameMinLength)]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = ValidationMessages.EmailRequired)]
        [EmailAddress(ErrorMessage = ValidationMessages.EmailInvalid)]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = ValidationMessages.PasswordRequired)]
        [MinLength(8, ErrorMessage = ValidationMessages.PasswordMinLength)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
            ErrorMessage = ValidationMessages.PasswordComplexity)]
        public string Password { get; set; } = "";
    }
}

