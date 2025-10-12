using Book_Rental.DTOs.Requests;
using Book_Rental.DTOs.Responses;
using Book_Rental.Entities;
using Book_Rental.Interfaces;

namespace Book_Rental.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;
        public BookService(IBookRepository repo) => _repo = repo;

        public async Task<IEnumerable<BookResponseDto>> GetAllAsync()
        {
            var books = await _repo.GetAllAsync();
            return books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Stock = b.Stock,
                UserId = b.UserId
            });
        }

        public async Task<BookResponseDto?> GetByIdAsync(Guid id)
        {
            var b = await _repo.GetByIdAsync(id);
            if (b == null) return null;

            return new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Stock = b.Stock,
                UserId = b.UserId
            };
        }

        public async Task AddAsync(BookRequestDto book)
        {
            var b = new Book
            {
                Title = book.Title,
                Author = book.Author,
                Stock = book.Stock
            };
            await _repo.AddAsync(b);
        }

        public async Task UpdateAsync(Guid id, BookRequestDto book)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) throw new Exception("Book not found.");

            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Stock = book.Stock;

            await _repo.UpdateAsync(existing);
        }

        public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);

        public async Task RentBookAsync(Guid bookId, Guid userId)
        {
            var book = await _repo.GetByIdAsync(bookId);
            if (book == null || book.Stock <= 0)
                throw new Exception("Book not available.");

            book.Stock--;
            book.UserId = userId;
            await _repo.UpdateAsync(book);
        }

        public async Task ReturnBookAsync(Guid bookId)
        {
            var book = await _repo.GetByIdAsync(bookId);
            if (book == null)
                throw new Exception("Book not found.");

            book.Stock++;
            book.UserId = null;
            await _repo.UpdateAsync(book);
        }
    }
}
