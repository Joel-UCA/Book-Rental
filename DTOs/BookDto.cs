namespace Book_Rental.DTOs
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public int Stock { get; set; }
        public Guid? UserId { get; set; }
    }
}
