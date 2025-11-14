using Book_Rental.Constants;
using Book_Rental.Data;
using Book_Rental.DTOs.Responses;
using Book_Rental.Entities;
using Book_Rental.Exceptions;
using Book_Rental.Interfaces;
using Book_Rental.Models;

namespace Book_Rental.Services
{
    public class BookRequestService : IBookRequestService
    {
        private readonly IBookRequestRepository _bookRequestRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRentalHistoryRepository _rentalHistoryRepository;
        private readonly AppDbContext _context;

        public BookRequestService(IBookRequestRepository bookRequestRepository, IBookRepository bookRepository, IUserRepository userRepository, IRentalHistoryRepository rentalHistoryRepository, AppDbContext context)
        {
            _bookRequestRepository = bookRequestRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _rentalHistoryRepository = rentalHistoryRepository;
            _context = context;
        }

        public async Task RequestBookAsync(Guid bookId, Guid userId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                throw new NotFoundException("Book", bookId);

            if (book.Stock <= 0)
                throw new BadRequestException("Book is not available for rental.");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User", userId);

            var request = new BookRequest
            {
                BookId = bookId,
                UserId = userId,
            };

            await _bookRequestRepository.AddAsync(request);
        }

        public async Task ApproveRequestAsync(Guid requestId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var request = await _bookRequestRepository.GetByIdAsync(requestId);
                if (request == null)
                    throw new NotFoundException("Book request", requestId);

                if (request.Status != RequestStatus.Pending)
                    throw new BadRequestException($"Cannot approve a request with status '{request.Status}'.");

                var book = await _bookRepository.GetByIdAsync(request.BookId);
                if (book == null)
                    throw new NotFoundException("Book", request.BookId);

                if (book.Stock <= 0)
                    throw new BadRequestException("Book is no longer available for approval.");

                request.Status = RequestStatus.Approved;
                await _bookRequestRepository.UpdateAsync(request);

                book.Stock--;
                await _bookRepository.UpdateAsync(book);

                var rentalHistory = new RentalHistory
                {
                    BookId = request.BookId,
                    UserId = request.UserId,
                };
                await _rentalHistoryRepository.AddAsync(rentalHistory);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RejectRequestAsync(Guid requestId)
        {
            var request = await _bookRequestRepository.GetByIdAsync(requestId);
            if (request == null)
                throw new NotFoundException("Book request", requestId);

            if (request.Status != RequestStatus.Pending)
                throw new BadRequestException($"Cannot reject a request with status '{request.Status}'.");

            request.Status = RequestStatus.Rejected;
            await _bookRequestRepository.UpdateAsync(request);
        }

        public async Task<IEnumerable<BookRequestResponseDto>> GetAllRequestsAsync()
        {
            var requests = await _bookRequestRepository.GetAllAsync();
            return requests.Select(r => new BookRequestResponseDto
            {
                Id = r.Id,
                BookId = r.BookId,
                UserId = r.UserId,
                RequestDate = r.RequestDate,
                Status = r.Status
            });
        }

        public async Task<PagedResult<BookRequestResponseDto>> GetAllRequestsPagedAsync(PaginationParams paginationParams)
        {
            var (requests, totalCount) = await _bookRequestRepository.GetAllPagedAsync(paginationParams);
            var requestDtos = requests.Select(r => new BookRequestResponseDto
            {
                Id = r.Id,
                BookId = r.BookId,
                UserId = r.UserId,
                RequestDate = r.RequestDate,
                Status = r.Status
            });

            return new PagedResult<BookRequestResponseDto>(requestDtos, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<IEnumerable<BookRequestResponseDto>> GetUserRequestsAsync(Guid userId)
        {
            var requests = await _bookRequestRepository.GetByUserIdAsync(userId);
            return requests.Select(r => new BookRequestResponseDto
            {
                Id = r.Id,
                BookId = r.BookId,
                UserId = r.UserId,
                RequestDate = r.RequestDate,
                Status = r.Status
            });
        }

        public async Task<IEnumerable<BookRequestResponseDto>> GetRequestsByStatusAsync(string status)
        {
            var requests = await _bookRequestRepository.GetByStatusAsync(status);
            return requests.Select(r => new BookRequestResponseDto
            {
                Id = r.Id,
                BookId = r.BookId,
                UserId = r.UserId,
                RequestDate = r.RequestDate,
                Status = r.Status
            });
        }

        public async Task CancelRequestAsync(Guid requestId, Guid userId)
        {
            var request = await _bookRequestRepository.GetByIdAsync(requestId);
            if (request == null)
                throw new NotFoundException("Book request", requestId);

            // Verify the request belongs to the user
            if (request.UserId != userId)
                throw new ForbiddenException("You can only cancel your own requests.");

            // Only allow canceling pending requests
            if (request.Status != RequestStatus.Pending)
                throw new BadRequestException("Only pending requests can be canceled.");

            await _bookRequestRepository.DeleteAsync(requestId);
        }
    }
}
