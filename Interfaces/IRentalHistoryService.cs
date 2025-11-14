using Book_Rental.DTOs.Responses;

namespace Book_Rental.Interfaces
{
    public interface IRentalHistoryService
    {
        Task<IEnumerable<RentalHistoryResponseDto>> GetRentalHistoryForUserAsync(Guid userId);
        Task<IEnumerable<RentalHistoryResponseDto>> GetAllRentalHistoriesAsync();
        Task<IEnumerable<RentalHistoryResponseDto>> GetActiveRentalsAsync();
        Task<object> GetRentalStatisticsAsync();
    }
}
