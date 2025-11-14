using Book_Rental.Entities;
using Book_Rental.Models;

namespace Book_Rental.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<(IEnumerable<User> items, int totalCount)> GetAllPagedAsync(PaginationParams paginationParams);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
