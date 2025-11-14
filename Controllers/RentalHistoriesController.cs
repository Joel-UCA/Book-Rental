using Book_Rental.Constants;
using Book_Rental.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Book_Rental.Controllers
{
    [Route("api/rental-histories")]
    [ApiController]
    [Authorize]
    public class RentalHistoriesController : ControllerBase
    {
        private readonly IRentalHistoryService _rentalHistoryService;

        public RentalHistoriesController(IRentalHistoryService rentalHistoryService)
        {
            _rentalHistoryService = rentalHistoryService;
        }

        [HttpGet("my-history")]
        [Authorize]
        public async Task<IActionResult> GetMyRentalHistory()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User identification not found.");

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
                return BadRequest("Invalid user identifier.");

            var history = await _rentalHistoryService.GetRentalHistoryForUserAsync(userId);
            return Ok(history);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetRentalHistoryForUser(Guid userId)
        {
            var history = await _rentalHistoryService.GetRentalHistoryForUserAsync(userId);
            return Ok(history);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllRentalHistories()
        {
            var histories = await _rentalHistoryService.GetAllRentalHistoriesAsync();
            return Ok(histories);
        }

        [HttpGet("active")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetActiveRentals()
        {
            var activeRentals = await _rentalHistoryService.GetActiveRentalsAsync();
            return Ok(activeRentals);
        }

        [HttpGet("statistics")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetStatistics()
        {
            var statistics = await _rentalHistoryService.GetRentalStatisticsAsync();
            return Ok(statistics);
        }
    }
}
