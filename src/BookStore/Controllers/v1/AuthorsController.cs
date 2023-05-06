using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
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
            var books = await bookService.GetBooksByAuthorAsync(id);

            return Ok(books);
        }
    }
}
