using Book_Rental.DTOs.Responses;
using Book_Rental.Models;

namespace Book_Rental.Interfaces
{
    public interface IBookRequestService
    {
        Task RequestBookAsync(Guid bookId, Guid userId);
        Task ApproveRequestAsync(Guid requestId);
        Task RejectRequestAsync(Guid requestId);
        Task<IEnumerable<BookRequestResponseDto>> GetAllRequestsAsync();
        Task<PagedResult<BookRequestResponseDto>> GetAllRequestsPagedAsync(PaginationParams paginationParams);
        Task<IEnumerable<BookRequestResponseDto>> GetUserRequestsAsync(Guid userId);
        Task<IEnumerable<BookRequestResponseDto>> GetRequestsByStatusAsync(string status);
        Task CancelRequestAsync(Guid requestId, Guid userId);
    }
}
