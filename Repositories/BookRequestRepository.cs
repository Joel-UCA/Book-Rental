using Book_Rental.Data;
using Book_Rental.Entities;
using Book_Rental.Interfaces;
using Book_Rental.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_Rental.Repositories
{
    public class BookRequestRepository : IBookRequestRepository
    {
        private readonly AppDbContext _context;

        public BookRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BookRequest bookRequest)
        {
            await _context.BookRequests.AddAsync(bookRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookRequest>> GetAllAsync()
        {
            return await _context.BookRequests
                .AsNoTracking()
                .Include(br => br.Book)
                .Include(br => br.User)
                .ToListAsync();
        }

        public async Task<(IEnumerable<BookRequest> items, int totalCount)> GetAllPagedAsync(PaginationParams paginationParams)
        {
            var query = _context.BookRequests
                .AsNoTracking()
                .Include(br => br.Book)
                .Include(br => br.User)
                .AsQueryable();
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<BookRequest?> GetByIdAsync(Guid id)
        {
            return await _context.BookRequests
                .Include(br => br.Book)
                .Include(br => br.User)
                .FirstOrDefaultAsync(br => br.Id == id);
        }

        public async Task UpdateAsync(BookRequest bookRequest)
        {
            _context.BookRequests.Update(bookRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookRequest>> GetByUserIdAsync(Guid userId)
        {
            return await _context.BookRequests
                .AsNoTracking()
                .Include(br => br.Book)
                .Include(br => br.User)
                .Where(br => br.UserId == userId)
                .OrderByDescending(br => br.RequestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookRequest>> GetByStatusAsync(string status)
        {
            return await _context.BookRequests
                .AsNoTracking()
                .Include(br => br.Book)
                .Include(br => br.User)
                .Where(br => br.Status == status)
                .OrderByDescending(br => br.RequestDate)
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var bookRequest = await _context.BookRequests.FindAsync(id);
            if (bookRequest != null)
            {
                _context.BookRequests.Remove(bookRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}
