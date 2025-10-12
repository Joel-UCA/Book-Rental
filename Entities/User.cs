namespace Book_Rental.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

        // Relationship - one user can rent many books
        public ICollection<Book>? Books { get; set; }

    }
}
