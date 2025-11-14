using Book_Rental.Entities;

namespace Book_Rental.Interfaces
{
    public interface IRentalHistoryRepository
    {
        Task AddAsync(RentalHistory rentalHistory);
        Task<RentalHistory> GetByBookIdAndNullReturnDateAsync(Guid bookId);
        Task UpdateAsync(RentalHistory rentalHistory);
        Task<IEnumerable<RentalHistory>> GetAllByUserIdAsync(Guid userId);
        Task<IEnumerable<RentalHistory>> GetAllAsync();
        Task<IEnumerable<RentalHistory>> GetActiveRentalsAsync();
        Task<int> GetTotalRentalsCountAsync();
        Task<int> GetActiveRentalsCountAsync();
    }
}
