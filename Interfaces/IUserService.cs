using Book_Rental.DTOs.Requests;
using Book_Rental.DTOs.Responses;
using Book_Rental.Models;

namespace Book_Rental.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllAsync();
        Task<PagedResult<UserResponseDto>> GetAllPagedAsync(PaginationParams paginationParams);
        Task<UserResponseDto?> GetByIdAsync(Guid id);
        Task RegisterUserAsync(UserRequestDto user);
        Task UpdateUserAsync(Guid id, UserRequestDto user);
        Task DeleteUserAsync(Guid id);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest);
        Task<UserResponseDto?> GetCurrentUserAsync(Guid userId);
        Task UpdateCurrentUserAsync(Guid userId, UpdateProfileRequestDto updateProfile);
        Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto changePassword);
    }
}
