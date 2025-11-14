using Book_Rental.Constants;
using System.ComponentModel.DataAnnotations;

namespace Book_Rental.Entities
{
    public class BookRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid BookId { get; set; }
        public Book Book { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = RequestStatus.Pending; // Pending, Approved, Rejected
    }
}
