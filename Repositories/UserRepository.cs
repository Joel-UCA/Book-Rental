using Book_Rental.Data;
using Book_Rental.Entities;
using Book_Rental.Interfaces;
using Book_Rental.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_Rental.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.AsNoTracking().ToListAsync();

        public async Task<(IEnumerable<User> items, int totalCount)> GetAllPagedAsync(PaginationParams paginationParams)
        {
            var query = _context.Users.AsNoTracking().AsQueryable();
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _context.Users.FindAsync(id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
