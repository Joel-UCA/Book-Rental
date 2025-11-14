using System.ComponentModel.DataAnnotations;

namespace Book_Rental.Entities
{
    public class RentalHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid BookId { get; set; }
        public Book Book { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public DateTime RentalDate { get; set; } = DateTime.UtcNow;

        public DateTime? ReturnDate { get; set; }
    }
}
