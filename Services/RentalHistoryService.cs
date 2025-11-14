using Book_Rental.DTOs.Responses;
using Book_Rental.Interfaces;

namespace Book_Rental.Services
{
    public class RentalHistoryService : IRentalHistoryService
    {
        private readonly IRentalHistoryRepository _rentalHistoryRepository;

        public RentalHistoryService(IRentalHistoryRepository rentalHistoryRepository)
        {
            _rentalHistoryRepository = rentalHistoryRepository;
        }

        public async Task<IEnumerable<RentalHistoryResponseDto>> GetRentalHistoryForUserAsync(Guid userId)
        {
            var rentalHistories = await _rentalHistoryRepository.GetAllByUserIdAsync(userId);
            return rentalHistories.Select(rh => new RentalHistoryResponseDto
            {
                Id = rh.Id,
                BookId = rh.BookId,
                UserId = rh.UserId,
                RentalDate = rh.RentalDate,
                ReturnDate = rh.ReturnDate
            });
        }

        public async Task<IEnumerable<RentalHistoryResponseDto>> GetAllRentalHistoriesAsync()
        {
            var rentalHistories = await _rentalHistoryRepository.GetAllAsync();
            return rentalHistories.Select(rh => new RentalHistoryResponseDto
            {
                Id = rh.Id,
                BookId = rh.BookId,
                UserId = rh.UserId,
                RentalDate = rh.RentalDate,
                ReturnDate = rh.ReturnDate
            });
        }

        public async Task<IEnumerable<RentalHistoryResponseDto>> GetActiveRentalsAsync()
        {
            var activeRentals = await _rentalHistoryRepository.GetActiveRentalsAsync();
            return activeRentals.Select(rh => new RentalHistoryResponseDto
            {
                Id = rh.Id,
                BookId = rh.BookId,
                UserId = rh.UserId,
                RentalDate = rh.RentalDate,
                ReturnDate = rh.ReturnDate
            });
        }

        public async Task<object> GetRentalStatisticsAsync()
        {
            var totalRentals = await _rentalHistoryRepository.GetTotalRentalsCountAsync();
            var activeRentals = await _rentalHistoryRepository.GetActiveRentalsCountAsync();
            var completedRentals = totalRentals - activeRentals;

            return new
            {
                TotalRentals = totalRentals,
                ActiveRentals = activeRentals,
                CompletedRentals = completedRentals
            };
        }
    }
}
