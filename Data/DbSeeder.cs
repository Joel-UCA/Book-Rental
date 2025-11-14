using Book_Rental.Constants;
using Book_Rental.Entities;
using Microsoft.EntityFrameworkCore;

namespace Book_Rental.Data
{
    public class DbSeeder
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public DbSeeder(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task SeedAsync()
        {
            // Ensure database is created and migrations are applied
            await _context.Database.MigrateAsync();

            // Check if admin user should be created
            var adminEmail = _configuration["AdminUser:Email"];
            var adminPassword = _configuration["AdminUser:Password"];

            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            {
                Console.WriteLine("⚠️  Admin user credentials not configured. Skipping admin user creation.");
                Console.WriteLine("   To create admin user, set AdminUser:Email and AdminUser:Password in User Secrets or Environment Variables.");
                return;
            }

            var existingAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);

            if (existingAdmin == null)
            {
                // Create admin user from configuration
                var adminUser = new User
                {
                    FullName = _configuration["AdminUser:FullName"] ?? "System Administrator",
                    Email = adminEmail,
                    Password = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                    Role = Roles.Admin
                };

                _context.Users.Add(adminUser);
                await _context.SaveChangesAsync();

                Console.WriteLine($"✓ Admin user created successfully!");
                Console.WriteLine($"  Email: {adminEmail}");
                Console.WriteLine($"  ⚠️  IMPORTANT: Change the admin password after first login!");
            }
            else
            {
                Console.WriteLine($"✓ Admin user already exists: {adminEmail}");
            }
        }
    }
}
