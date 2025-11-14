using Book_Rental.Data;
using Book_Rental.DTOs.Requests;
using Book_Rental.DTOs.Responses;
using Book_Rental.Entities;
using Book_Rental.Exceptions;
using Book_Rental.Interfaces;
using Book_Rental.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_Rental.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;
        private readonly IRentalHistoryRepository _rentalHistoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;

        public BookService(IBookRepository repo, IRentalHistoryRepository rentalHistoryRepository, IUserRepository userRepository, AppDbContext context)
        {
             _repo = repo;
            _rentalHistoryRepository = rentalHistoryRepository;
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<IEnumerable<BookResponseDto>> GetAllAsync()
        {
            var books = await _repo.GetAllAsync();
            return books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Stock = b.Stock
            });
        }

        public async Task<PagedResult<BookResponseDto>> GetAllPagedAsync(PaginationParams paginationParams)
        {
            var (books, totalCount) = await _repo.GetAllPagedAsync(paginationParams);
            var bookDtos = books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Stock = b.Stock
            });

            return new PagedResult<BookResponseDto>(bookDtos, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
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
                Stock = b.Stock
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
            if (existing == null) throw new NotFoundException("Book", id);

            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Stock = book.Stock;

            await _repo.UpdateAsync(existing);
        }

        public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);

        public async Task RentBookAsync(Guid bookId, Guid userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validate user exists
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException("User", userId);

                // Validate book exists
                var book = await _repo.GetByIdAsync(bookId);
                if (book == null)
                    throw new NotFoundException("Book", bookId);

                if (book.Stock <= 0)
                    throw new BadRequestException("Book is not available for rent.");

                // Decrease stock
                book.Stock--;
                await _repo.UpdateAsync(book);

                // Create RentalHistory entry
                var rentalHistory = new RentalHistory
                {
                    BookId = bookId,
                    UserId = userId,
                    RentalDate = DateTime.UtcNow
                };
                await _rentalHistoryRepository.AddAsync(rentalHistory);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ReturnBookAsync(Guid bookId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var book = await _repo.GetByIdAsync(bookId);
                if (book == null)
                    throw new NotFoundException("Book", bookId);

                // Find active rental for this book
                var rentalHistory = await _rentalHistoryRepository.GetByBookIdAndNullReturnDateAsync(bookId);
                if (rentalHistory == null)
                    throw new BadRequestException("No active rental found for this book.");

                // Verify user exists (data integrity check)
                var user = await _userRepository.GetByIdAsync(rentalHistory.UserId);
                if (user == null)
                    throw new BadRequestException("Invalid rental: user associated with this rental no longer exists.");

                // Mark as returned
                rentalHistory.ReturnDate = DateTime.UtcNow;
                await _rentalHistoryRepository.UpdateAsync(rentalHistory);

                // Increase stock
                book.Stock++;
                await _repo.UpdateAsync(book);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<BookResponseDto>> SearchBooksAsync(string query)
        {
            var books = await _repo.SearchBooksAsync(query);
            return books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Stock = b.Stock
            });
        }

        public async Task<IEnumerable<BookResponseDto>> GetAvailableBooksAsync()
        {
            var books = await _repo.GetAvailableBooksAsync();
            return books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Stock = b.Stock
            });
        }
    }
}
