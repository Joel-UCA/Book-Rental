using Book_Rental.Constants;
using System.ComponentModel.DataAnnotations;

namespace Book_Rental.DTOs.Requests
{
    public class UpdateProfileRequestDto
    {
        [Required(ErrorMessage = ValidationMessages.FullNameRequired)]
        [MinLength(2, ErrorMessage = ValidationMessages.FullNameMinLength)]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = ValidationMessages.EmailRequired)]
        [EmailAddress(ErrorMessage = ValidationMessages.EmailInvalid)]
        public string Email { get; set; } = "";
    }
}
