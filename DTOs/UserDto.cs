namespace Book_Rental.DTOs
{
    public class UserDto
    {
        //public Guid Id { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = ""; // used for register/update
    }
}
