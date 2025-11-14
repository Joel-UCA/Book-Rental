using Book_Rental.Constants;
using System.ComponentModel.DataAnnotations;

namespace Book_Rental.DTOs.Requests
{
    public class ChangePasswordRequestDto
    {
        [Required(ErrorMessage = ValidationMessages.CurrentPasswordRequired)]
        public string CurrentPassword { get; set; } = "";

        [Required(ErrorMessage = ValidationMessages.NewPasswordRequired)]
        [MinLength(8, ErrorMessage = ValidationMessages.NewPasswordMinLength)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
            ErrorMessage = ValidationMessages.PasswordComplexity)]
        public string NewPassword { get; set; } = "";

        [Required(ErrorMessage = ValidationMessages.ConfirmPasswordRequired)]
        [Compare("NewPassword", ErrorMessage = ValidationMessages.PasswordsDoNotMatch)]
        public string ConfirmNewPassword { get; set; } = "";
    }
}
