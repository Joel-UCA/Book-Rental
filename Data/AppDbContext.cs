using System.Collections.Generic;
using Book_Rental.Entities;
using Microsoft.EntityFrameworkCore;

namespace Book_Rental.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
