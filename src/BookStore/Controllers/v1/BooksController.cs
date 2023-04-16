using BookStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BookStore.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
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
    }
}
