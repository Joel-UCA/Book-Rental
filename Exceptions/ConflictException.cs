namespace Book_Rental.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {
        }
    }
}
