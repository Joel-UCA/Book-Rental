namespace Book_Rental.DTOs.Responses
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = "";
        public string? Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
