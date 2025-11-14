using Book_Rental.Entities;
using Book_Rental.Models;

namespace Book_Rental.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<(IEnumerable<Book> items, int totalCount)> GetAllPagedAsync(PaginationParams paginationParams);
        Task<Book?> GetByIdAsync(Guid id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Book>> SearchBooksAsync(string query);
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
    }
}
