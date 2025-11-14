using Book_Rental.Entities;
using Book_Rental.Models;

namespace Book_Rental.Interfaces
{
    public interface IBookRequestRepository
    {
        Task AddAsync(BookRequest bookRequest);
        Task<IEnumerable<BookRequest>> GetAllAsync();
        Task<(IEnumerable<BookRequest> items, int totalCount)> GetAllPagedAsync(PaginationParams paginationParams);
        Task<BookRequest> GetByIdAsync(Guid id);
        Task UpdateAsync(BookRequest bookRequest);
        Task<IEnumerable<BookRequest>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<BookRequest>> GetByStatusAsync(string status);
        Task DeleteAsync(Guid id);
    }
}
