using Book_Rental.Data;
using Book_Rental.Entities;
using Book_Rental.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Book_Rental.Repositories
{
    public class RentalHistoryRepository : IRentalHistoryRepository
    {
        private readonly AppDbContext _context;

        public RentalHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RentalHistory rentalHistory)
        {
            await _context.RentalHistories.AddAsync(rentalHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<RentalHistory?> GetByBookIdAndNullReturnDateAsync(Guid bookId)
        {
            return await _context.RentalHistories
                .Include(rh => rh.Book)
                .Include(rh => rh.User)
                .FirstOrDefaultAsync(rh => rh.BookId == bookId && rh.ReturnDate == null);
        }

        public async Task UpdateAsync(RentalHistory rentalHistory)
        {
            _context.RentalHistories.Update(rentalHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RentalHistory>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.RentalHistories
                .AsNoTracking()
                .Include(rh => rh.Book)
                .Include(rh => rh.User)
                .Where(rh => rh.UserId == userId)
                .OrderByDescending(rh => rh.RentalDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<RentalHistory>> GetAllAsync()
        {
            return await _context.RentalHistories
                .AsNoTracking()
                .Include(rh => rh.Book)
                .Include(rh => rh.User)
                .OrderByDescending(rh => rh.RentalDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<RentalHistory>> GetActiveRentalsAsync()
        {
            return await _context.RentalHistories
                .AsNoTracking()
                .Include(rh => rh.Book)
                .Include(rh => rh.User)
                .Where(rh => rh.ReturnDate == null)
                .OrderByDescending(rh => rh.RentalDate)
                .ToListAsync();
        }

        public async Task<int> GetTotalRentalsCountAsync()
        {
            return await _context.RentalHistories.CountAsync();
        }

        public async Task<int> GetActiveRentalsCountAsync()
        {
            return await _context.RentalHistories
                .Where(rh => rh.ReturnDate == null)
                .CountAsync();
        }
    }
}
