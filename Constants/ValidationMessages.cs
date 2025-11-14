namespace Book_Rental.Constants
{
    /// <summary>
    /// Common validation error messages
    /// </summary>
    public static class ValidationMessages
    {
        // User validation messages
        public const string EmailRequired = "Email is required";
        public const string EmailInvalid = "Invalid email address";
        public const string PasswordRequired = "Password is required";
        public const string PasswordMinLength = "Password must be at least 8 characters";
        public const string FullNameRequired = "Full name is required";
        public const string FullNameMinLength = "Full name must be at least 2 characters";

        // Book validation messages
        public const string TitleRequired = "Book title is required";
        public const string TitleMinLength = "Title must be at least 1 character";
        public const string AuthorRequired = "Author name is required";
        public const string AuthorMinLength = "Author name must be at least 1 character";
        public const string StockRequired = "Stock is required";
        public const string StockMinValue = "Stock must be at least 0";

        // Password change validation messages
        public const string CurrentPasswordRequired = "Current password is required";
        public const string NewPasswordRequired = "New password is required";
        public const string NewPasswordMinLength = "New password must be at least 8 characters";
        public const string PasswordComplexity = "Password must contain at least 8 characters, including uppercase, lowercase, number, and special character";
        public const string ConfirmPasswordRequired = "Password confirmation is required";
        public const string PasswordsDoNotMatch = "Passwords do not match";
    }
}
