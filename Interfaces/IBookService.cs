using Book_Rental.DTOs;

namespace Book_Rental.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<BookDto?> GetByIdAsync(Guid id);
        Task AddAsync(BookDto book);
        Task UpdateAsync(BookDto book);
        Task DeleteAsync(Guid id);
        Task RentBookAsync(Guid bookId, Guid userId);
        Task ReturnBookAsync(Guid bookId);
    }
}
