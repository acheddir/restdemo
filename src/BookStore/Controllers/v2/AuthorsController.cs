using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers.v2
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiVersion("2.1")]
    public class AuthorsController : ControllerBase
    {
        private readonly IBookService bookService;

        public AuthorsController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet("{id}/books")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var books = await bookService.GetBooksByAuthorv2Async(id);

            return Ok(books);
        }
    }
}
