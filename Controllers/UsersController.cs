using Book_Rental.Constants;
using Book_Rental.DTOs.Requests;
using Book_Rental.Interfaces;
using Book_Rental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Book_Rental.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        public UsersController(IUserService service) => _service = service;

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRequestDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.RegisterUserAsync(user);
            return StatusCode(201, new { Message = "User registered successfully." });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _service.LoginAsync(loginRequest);
            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _service.GetCurrentUserAsync(userId);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateCurrentUser(UpdateProfileRequestDto updateProfile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _service.UpdateCurrentUserAsync(userId, updateProfile);
            return Ok(new { Message = "Profile updated successfully." });
        }

        [HttpPut("me/password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto changePassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _service.ChangePasswordAsync(userId, changePassword);
            return Ok(new { Message = "Password changed successfully." });
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAll([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
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
                var pagedResult = await _service.GetAllPagedAsync(paginationParams);
                return Ok(pagedResult);
            }

            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _service.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Update(Guid id, UserRequestDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateUserAsync(id, user);
            return Ok(new { Message = "User updated successfully." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteUserAsync(id);
            return NoContent();
        }
    }
}