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
        public DbSet<BookRequest> BookRequests { get; set; }
        public DbSet<RentalHistory> RentalHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // BookRequest indexes
            modelBuilder.Entity<BookRequest>()
                .HasIndex(br => br.Status);

            modelBuilder.Entity<BookRequest>()
                .HasIndex(br => br.UserId);

            modelBuilder.Entity<BookRequest>()
                .HasIndex(br => br.BookId);

            modelBuilder.Entity<BookRequest>()
                .HasIndex(br => new { br.UserId, br.Status });

            // RentalHistory indexes
            modelBuilder.Entity<RentalHistory>()
                .HasIndex(rh => rh.UserId);

            modelBuilder.Entity<RentalHistory>()
                .HasIndex(rh => rh.BookId);

            modelBuilder.Entity<RentalHistory>()
                .HasIndex(rh => new { rh.BookId, rh.ReturnDate });

            modelBuilder.Entity<RentalHistory>()
                .HasIndex(rh => new { rh.UserId, rh.ReturnDate });
        }
    }
}
