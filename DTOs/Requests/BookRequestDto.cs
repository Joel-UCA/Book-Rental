using Book_Rental.Constants;
using System.ComponentModel.DataAnnotations;

namespace Book_Rental.DTOs.Requests
{
    public class BookRequestDto
    {
        [Required(ErrorMessage = ValidationMessages.TitleRequired)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = ValidationMessages.TitleMinLength)]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = ValidationMessages.AuthorRequired)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = ValidationMessages.AuthorMinLength)]
        public string Author { get; set; } = "";

        [Required(ErrorMessage = ValidationMessages.StockRequired)]
        [Range(0, int.MaxValue, ErrorMessage = ValidationMessages.StockMinValue)]
        public int Stock { get; set; }
    }
}
