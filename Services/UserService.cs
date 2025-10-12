using Book_Rental.DTOs;
using Book_Rental.Entities;
using Book_Rental.Interfaces;

namespace Book_Rental.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email
            });
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return null;

            return new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email
            };
        }

        public async Task RegisterUserAsync(UserDto user)
        {
            // Check duplicate
            var existing = await _repo.GetByEmailAsync(user.Email);
            if (existing != null)
                throw new Exception("User already exists with this email.");

            var newUser = new User
            {
                FullName = user.FullName,
                Email = user.Email,
                Password = user.Password // for now, plain text (you can hash later)
            };

            await _repo.AddAsync(newUser);
        }

        public async Task UpdateUserAsync(UserDto user)
        {
            var existing = await _repo.GetByIdAsync(user.Id);
            if (existing == null)
                throw new Exception("User not found.");

            existing.FullName = user.FullName;
            existing.Email = user.Email;
            if (!string.IsNullOrEmpty(user.Password))
                existing.Password = user.Password;

            await _repo.UpdateAsync(existing);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
