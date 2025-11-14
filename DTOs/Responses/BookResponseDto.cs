namespace Book_Rental.DTOs.Responses
{
    public class BookResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public int Stock { get; set; }
    }
}
