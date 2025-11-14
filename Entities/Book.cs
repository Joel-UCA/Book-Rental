using System.ComponentModel.DataAnnotations;

namespace Book_Rental.Entities
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string Author { get; set; } = "";

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        // Rental tracking is now handled solely through RentalHistory table
        // This allows multiple users to rent copies of the same book (when Stock > 1)
    }
}
