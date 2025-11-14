using Book_Rental.DTOs.Requests;
using Book_Rental.DTOs.Responses;
using Book_Rental.Models;

namespace Book_Rental.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookResponseDto>> GetAllAsync();
        Task<PagedResult<BookResponseDto>> GetAllPagedAsync(PaginationParams paginationParams);
        Task<BookResponseDto?> GetByIdAsync(Guid id);
        Task AddAsync(BookRequestDto book);
        Task UpdateAsync(Guid id, BookRequestDto book);
        Task DeleteAsync(Guid id);
        Task RentBookAsync(Guid bookId, Guid userId);
        Task ReturnBookAsync(Guid bookId);
        Task<IEnumerable<BookResponseDto>> SearchBooksAsync(string query);
        Task<IEnumerable<BookResponseDto>> GetAvailableBooksAsync();
    }
}
