using Book_Rental.DTOs.Requests;
using Book_Rental.DTOs.Responses;

namespace Book_Rental.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllAsync();
        Task<UserResponseDto?> GetByIdAsync(Guid id);
        Task RegisterUserAsync(UserRequestDto user);
        Task UpdateUserAsync(Guid id, UserRequestDto user);
        Task DeleteUserAsync(Guid id);
    }
}
