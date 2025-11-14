namespace Book_Rental.DTOs.Responses
{
    public class BookRequestResponseDto
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }
}
