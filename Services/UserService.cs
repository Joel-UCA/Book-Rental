using Book_Rental.Constants;
using Book_Rental.DTOs.Requests;
using Book_Rental.DTOs.Responses;
using Book_Rental.Entities;
using Book_Rental.Exceptions;
using Book_Rental.Interfaces;
using Book_Rental.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace Book_Rental.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public UserService(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role
            });
        }

        public async Task<PagedResult<UserResponseDto>> GetAllPagedAsync(PaginationParams paginationParams)
        {
            var (users, totalCount) = await _repo.GetAllPagedAsync(paginationParams);
            var userDtos = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role
            });

            return new PagedResult<UserResponseDto>(userDtos, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<UserResponseDto?> GetByIdAsync(Guid id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return null;

            return new UserResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role
            };
        }

        public async Task RegisterUserAsync(UserRequestDto user)
        {
            var existing = await _repo.GetByEmailAsync(user.Email);
            if (existing != null)
                throw new ConflictException("User already exists with this email.");

            var newUser = new User
            {
                FullName = user.FullName,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            };

            await _repo.AddAsync(newUser);
        }

        public async Task UpdateUserAsync(Guid id, UserRequestDto user)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException("User", id);

            existing.FullName = user.FullName;
            existing.Email = user.Email;
            if (!string.IsNullOrEmpty(user.Password))
                existing.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _repo.UpdateAsync(existing);
        }

        public async Task DeleteUserAsync(Guid id) => await _repo.DeleteAsync(id);

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest)
        {
            var user = await _repo.GetByEmailAsync(loginRequest.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
                throw new UnauthorizedException("Invalid credentials.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured."));
            var expirationHours = _config.GetValue<int>("Jwt:ExpirationHours", 1);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(expirationHours),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new LoginResponseDto { Token = tokenHandler.WriteToken(token) };
        }

        public async Task<UserResponseDto?> GetCurrentUserAsync(Guid userId)
        {
            var user = await _repo.GetByIdAsync(userId);
            if (user == null) return null;

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task UpdateCurrentUserAsync(Guid userId, UpdateProfileRequestDto updateProfile)
        {
            var user = await _repo.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User", userId);

            // Check if email is being changed and if it's already taken by another user
            if (user.Email != updateProfile.Email)
            {
                var existingUser = await _repo.GetByEmailAsync(updateProfile.Email);
                if (existingUser != null && existingUser.Id != userId)
                    throw new ConflictException("Email is already in use by another user.");
            }

            user.FullName = updateProfile.FullName;
            user.Email = updateProfile.Email;

            await _repo.UpdateAsync(user);
        }

        public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto changePassword)
        {
            var user = await _repo.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User", userId);

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(changePassword.CurrentPassword, user.Password))
                throw new BadRequestException("Current password is incorrect.");

            // Update password
            user.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);

            await _repo.UpdateAsync(user);
        }
    }
}
