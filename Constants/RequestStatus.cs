namespace Book_Rental.Constants
{
    /// <summary>
    /// Defines possible statuses for book requests
    /// </summary>
    public static class RequestStatus
    {
        /// <summary>
        /// Request is waiting for admin approval
        /// </summary>
        public const string Pending = "Pending";

        /// <summary>
        /// Request has been approved and book has been rented
        /// </summary>
        public const string Approved = "Approved";

        /// <summary>
        /// Request has been rejected by admin
        /// </summary>
        public const string Rejected = "Rejected";
    }
}
