namespace Book_Rental.Entities
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public int Stock { get; set; }

        // Relationship - optional user who rented it
        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }
}
