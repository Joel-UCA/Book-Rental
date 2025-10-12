using Book_Rental.DTOs;
using Book_Rental.Entities;
using Book_Rental.Interfaces;

namespace Book_Rental.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;
        public BookService(IBookRepository repo) => _repo = repo;

        public async Task<IEnumerable<BookDto>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Stock = b.Stock,
                UserId = b.UserId
            });

        public async Task<BookDto?> GetByIdAsync(Guid id)
        {
            var b = await _repo.GetByIdAsync(id);
            return b == null ? null : new BookDto { Id = b.Id, Title = b.Title, Author = b.Author, Stock = b.Stock, UserId = b.UserId };
        }

        public async Task AddAsync(BookDto book)
        {
            var b = new Book { Title = book.Title, Author = book.Author, Stock = book.Stock };
            await _repo.AddAsync(b);
        }

        public async Task UpdateAsync(BookDto book)
        {
            var existing = await _repo.GetByIdAsync(book.Id);
            if (existing == null) return;

            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Stock = book.Stock;
            await _repo.UpdateAsync(existing);
        }

        public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);

        public async Task RentBookAsync(Guid bookId, Guid userId)
        {
            var b = await _repo.GetByIdAsync(bookId);
            if (b == null || b.Stock <= 0) throw new Exception("Book not available.");

            b.Stock--;
            b.UserId = userId;
            await _repo.UpdateAsync(b);
        }

        public async Task ReturnBookAsync(Guid bookId)
        {
            var b = await _repo.GetByIdAsync(bookId);
            if (b == null) throw new Exception("Book not found.");

            b.Stock++;
            b.UserId = null;
            await _repo.UpdateAsync(b);
        }
    }
}
