namespace Book_Rental.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}
