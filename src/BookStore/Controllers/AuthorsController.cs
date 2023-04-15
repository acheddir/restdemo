using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/v1/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreContext context;

        public AuthorsController(BookStoreContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}/books")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var books = await context.Books
                .Where(b => !b.IsDeleted && b.AuthorId == id)
                .Select(b => new BookResponse(b.ISBN, b.Title, b.Year))
                .ToListAsync();

            return Ok(books);
        }
    }
}
