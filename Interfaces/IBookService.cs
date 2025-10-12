using Book_Rental.DTOs.Requests;
using Book_Rental.DTOs.Responses;

namespace Book_Rental.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookResponseDto>> GetAllAsync();
        Task<BookResponseDto?> GetByIdAsync(Guid id);
        Task AddAsync(BookRequestDto book);
        Task UpdateAsync(Guid id, BookRequestDto book);
        Task DeleteAsync(Guid id);
        Task RentBookAsync(Guid bookId, Guid userId);
        Task ReturnBookAsync(Guid bookId);
    }
}
