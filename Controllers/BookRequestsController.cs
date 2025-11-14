using Book_Rental.Constants;
using Book_Rental.Interfaces;
using Book_Rental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Book_Rental.Controllers
{
    [Route("api/book-requests")]
    [ApiController]
    [Authorize]
    public class BookRequestsController : ControllerBase
    {
        private readonly IBookRequestService _bookRequestService;

        public BookRequestsController(IBookRequestService bookRequestService)
        {
            _bookRequestService = bookRequestService;
        }

        [HttpPost("{bookId}")]
        [Authorize]
        public async Task<IActionResult> RequestBook(Guid bookId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { Message = "User identification not found." });

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
                return BadRequest(new { Message = "Invalid user identifier." });

            await _bookRequestService.RequestBookAsync(bookId, userId);
            return StatusCode(201, new { Message = "Book request submitted successfully." });
        }

        [HttpGet("my-requests")]
        [Authorize]
        public async Task<IActionResult> GetMyRequests()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var requests = await _bookRequestService.GetUserRequestsAsync(userId);
            return Ok(requests);
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetRequestsByStatus(string status)
        {
            var requests = await _bookRequestService.GetRequestsByStatusAsync(status);
            return Ok(requests);
        }

        [HttpDelete("{requestId}")]
        [Authorize]
        public async Task<IActionResult> CancelRequest(Guid requestId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _bookRequestService.CancelRequestAsync(requestId, userId);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllRequests([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                if (pageNumber.Value < 1)
                    return BadRequest(new { Message = "Page number must be greater than 0." });
                
                if (pageSize.Value < 1 || pageSize.Value > 100)
                    return BadRequest(new { Message = "Page size must be between 1 and 100." });
                
                var paginationParams = new PaginationParams
                {
                    PageNumber = pageNumber.Value,
                    PageSize = pageSize.Value
                };
                var pagedResult = await _bookRequestService.GetAllRequestsPagedAsync(paginationParams);
                return Ok(pagedResult);
            }

            var requests = await _bookRequestService.GetAllRequestsAsync();
            return Ok(requests);
        }

        [HttpPut("{requestId}/approve")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ApproveRequest(Guid requestId)
        {
            await _bookRequestService.ApproveRequestAsync(requestId);
            return Ok(new { Message = "Request approved successfully." });
        }

        [HttpPut("{requestId}/reject")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> RejectRequest(Guid requestId)
        {
            await _bookRequestService.RejectRequestAsync(requestId);
            return Ok(new { Message = "Request rejected successfully." });
        }
    }
}
