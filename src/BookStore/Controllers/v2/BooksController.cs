using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Controllers.v2
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiVersion("2.1")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [SwaggerOperation(
            Summary = "Get many books",
            Description = "Get many books using paging, sorting and filtering")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Returns many books")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized")]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] SieveModel sieveModel)
        {
            var booksResponse = await bookService.GetBooksAsync(sieveModel);

            return Ok(booksResponse);
        }

        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetByIsbnAsync(string isbn)
        {
            var bookResponse = await bookService.GetBookAsync(isbn);

            return Ok(bookResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBookCommand command)
        {
            var bookResponse = await bookService.CreateBookAsync(command);

            return Created($"api/v1/books/{bookResponse.Isbn}", bookResponse);
        }

        [HttpPut("{isbn}")]
        public async Task<IActionResult> UpdateAsync(string isbn, UpdateBookCommand command)
        {
            var bookResponse = await bookService.UpdateBookAsync(isbn, command);

            return Ok(bookResponse);
        }

        [HttpDelete("{isbn}")]
        [MapToApiVersion("2.1")]
        public async Task<IActionResult> DeleteAsync(string isbn)
        {
            await bookService.DeleteBookAsync(isbn);

            return Ok();
        }
    }
}
