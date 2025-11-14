namespace Book_Rental.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        {
        }
    }
}
