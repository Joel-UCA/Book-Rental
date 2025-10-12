using Book_Rental.DTOs;
using Book_Rental.DTOs.Requests;
using Book_Rental.DTOs.Responses;
using Book_Rental.Entities;
using Book_Rental.Interfaces;

namespace Book_Rental.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email
            });
        }

        public async Task<UserResponseDto?> GetByIdAsync(Guid id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return null;

            return new UserResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email
            };
        }

        public async Task RegisterUserAsync(UserRequestDto user)
        {
            var existing = await _repo.GetByEmailAsync(user.Email);
            if (existing != null)
                throw new Exception("User already exists with this email.");

            var newUser = new User
            {
                FullName = user.FullName,
                Email = user.Email,
                Password = user.Password
            };

            await _repo.AddAsync(newUser);
        }

        public async Task UpdateUserAsync(Guid id, UserRequestDto user)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("User not found.");

            existing.FullName = user.FullName;
            existing.Email = user.Email;
            if (!string.IsNullOrEmpty(user.Password))
                existing.Password = user.Password;

            await _repo.UpdateAsync(existing);
        }

        public async Task DeleteUserAsync(Guid id) => await _repo.DeleteAsync(id);
    }
}
