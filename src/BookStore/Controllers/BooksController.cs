using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Sieve.Services;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/v1/books")]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreContext context;
        private readonly SieveProcessor sieveProcessor;

        public BooksController(BookStoreContext context, SieveProcessor sieveProcessor)
        {
            this.context = context;
            this.sieveProcessor = sieveProcessor;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAsync([FromQuery] PagingParameters pagingParameters)
        //{
        //    var bookQueryBase = context.Books
        //        .Where(b => !b.IsDeleted)
        //        .Select(b => new BookResponse(b.ISBN, b.Title, b.Year));

        //    var pagedBooks = await PagedList<BookResponse>.CreateAsync(bookQueryBase, pagingParameters.Page, pagingParameters.Size);

        //    return Ok(pagedBooks);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] SieveModel sieveModel)
        {
            var bookQuery = context.Books
                .Where(b => !b.IsDeleted);

            var books = await sieveProcessor.Apply(sieveModel, bookQuery)
                .Select(b => new BookResponse(b.ISBN, b.Title, b.Year))
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetByIsbnAsync(string isbn)
        {
            return await context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn && !b.IsDeleted)
                is Book bookEntity
                    ? Ok(new BookResponse(bookEntity.ISBN, bookEntity.Title, bookEntity.Year))
                    : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBookCommand command)
        {
            var bookEntity = context.Books.Add(new Book { ISBN = command.ISBN, Title = command.Title, Year = command.Year, IsDeleted = false }).Entity;

            await context.SaveChangesAsync();

            var bookResponse = new BookResponse(bookEntity.ISBN, bookEntity.Title, bookEntity.Year);

            return Created($"api/v1/books/{bookResponse.ISBN}", bookResponse);
        }

        [HttpDelete("{isbn}")]
        public async Task<IActionResult> DeleteAsync(string isbn)
        {
            if (await context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn && !b.IsDeleted) is Book book)
            {
                // Soft Delete
                book.IsDeleted = true;

                await context.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }

        [HttpPut("{isbn}")]
        public async Task<IActionResult> UpdateAsync(string isbn, UpdateBookCommand command)
        {
            var bookEntity = await context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn && !b.IsDeleted);

            if (bookEntity is null)
                return NotFound();

            bookEntity.ISBN = command.ISBN;
            bookEntity.Title = command.Title;
            bookEntity.Year = command.Year;

            /* -- Non Idempotent check
             var bookEntry = context.ChangeTracker.Entries<Book>().FirstOrDefault(b => b.Entity.ISBN == isbn);
             if (bookEntry is not null && bookEntry.State == EntityState.Unchanged)
                 return Results.BadRequest(new { message = "No changes detected." });
            */

            await context.SaveChangesAsync();

            var bookResponse = new BookResponse(bookEntity.ISBN, bookEntity.Title, bookEntity.Year);

            return Ok(bookResponse);
        }
    }
}
