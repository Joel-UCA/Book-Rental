using Book_Rental.Data;
using Book_Rental.Entities;
using Book_Rental.Interfaces;
using Book_Rental.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_Rental.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Book>> GetAllAsync() => await _context.Books.AsNoTracking().ToListAsync();

        public async Task<(IEnumerable<Book> items, int totalCount)> GetAllPagedAsync(PaginationParams paginationParams)
        {
            var query = _context.Books.AsNoTracking().AsQueryable();
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Book?> GetByIdAsync(Guid id) => await _context.Books.FindAsync(id);

        public async Task AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string query)
        {
            return await _context.Books
                .AsNoTracking()
                .Where(b => b.Title.Contains(query) || b.Author.Contains(query))
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _context.Books
                .AsNoTracking()
                .Where(b => b.Stock > 0)
                .ToListAsync();
        }
    }
}
