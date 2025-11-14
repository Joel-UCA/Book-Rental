using Book_Rental.Constants;
using Book_Rental.DTOs.Requests;
using Book_Rental.Interfaces;
using Book_Rental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book_Rental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;
        public BooksController(IBookService service) => _service = service;

        [HttpGet]
        [AllowAnonymous] // Allow anonymous users to view all books
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
        [AllowAnonymous] // Allow anonymous users to view a book by id
        public async Task<IActionResult> Get(Guid id) => Ok(await _service.GetByIdAsync(id));

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query cannot be empty.");

            var books = await _service.SearchBooksAsync(query);
            return Ok(books);
        }

        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailable()
        {
            var books = await _service.GetAvailableBooksAsync();
            return Ok(books);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Add(BookRequestDto bookResponse)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.AddAsync(bookResponse);
            return StatusCode(201, new { Message = "Book added successfully." });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Update(Guid id, BookRequestDto book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateAsync(id, book);
            return Ok(new { Message = "Book updated successfully." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/rent/{userId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Rent(Guid id, Guid userId)
        {
            await _service.RentBookAsync(id, userId);
            return Ok(new { Message = "Book rented successfully." });
        }

        [HttpPost("{id}/return")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Return(Guid id)
        {
            await _service.ReturnBookAsync(id);
            return Ok(new { Message = "Book returned successfully." });
        }
    }
}
