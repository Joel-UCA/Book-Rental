using Book_Rental.DTOs;
using Book_Rental.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book_Rental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;
        public BooksController(IBookService service) => _service = service;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
        [HttpGet("{id}")] public async Task<IActionResult> Get(Guid id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Add(BookDto book)
        {
            await _service.AddAsync(book);
            return Ok("Book added successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(BookDto book)
        {
            await _service.UpdateAsync(book);
            return Ok("Book updated.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok("Book deleted.");
        }

        [HttpPost("{id}/rent/{userId}")]
        public async Task<IActionResult> Rent(Guid id, Guid userId)
        {
            await _service.RentBookAsync(id, userId);
            return Ok("Book rented.");
        }

        [HttpPost("{id}/return")]
        public async Task<IActionResult> Return(Guid id)
        {
            await _service.ReturnBookAsync(id);
            return Ok("Book returned.");
        }
    }
}
