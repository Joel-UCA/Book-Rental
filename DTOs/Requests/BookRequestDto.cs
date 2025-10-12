namespace Book_Rental.DTOs.Requests
{
    public class BookRequestDto
    {
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public int Stock { get; set; }
    }
}
