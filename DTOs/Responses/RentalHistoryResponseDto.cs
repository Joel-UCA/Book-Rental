namespace Book_Rental.DTOs.Responses
{
    public class RentalHistoryResponseDto
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
