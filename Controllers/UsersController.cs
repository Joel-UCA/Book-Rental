using Book_Rental.DTOs;
using Book_Rental.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book_Rental.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        public UsersController(IUserService service) => _service = service;

        // Register new user
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto user)
        {
            try
            {
                await _service.RegisterUserAsync(user);
                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Get all users
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        // Get by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        // Update user
        [HttpPut]
        public async Task<IActionResult> Update(UserDto user)
        {
            try
            {
                await _service.UpdateUserAsync(user);
                return Ok("User updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete user
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteUserAsync(id);
            return Ok("User deleted successfully.");
        }
    }
}
