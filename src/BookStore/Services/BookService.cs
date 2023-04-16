using FluentValidation;
using Sieve.Models;
using Sieve.Services;
using System.Text.RegularExpressions;

namespace BookStore.Services
{
    public partial class BookService : IBookService
    {
        private readonly BookStoreContext context;
        private readonly SieveProcessor sieveProcessor;
        private readonly IValidator<CreateBookCommand> validator;

        public BookService(
            BookStoreContext context,
            SieveProcessor sieveProcessor,
            IValidator<CreateBookCommand> validator)
        {
            this.context = context;
            this.sieveProcessor = sieveProcessor;
            this.validator = validator;
        }

        public async Task<BookResponse> CreateBookAsync(CreateBookCommand command)
        {
            var result = await validator.ValidateAsync(command);

            if (!result.IsValid)
            {
                throw new Exceptions.ValidationException(result.Errors);
            }

            var author = await context.Authors
                .FirstOrDefaultAsync(a => a.Name == command.Author);

            if (author == null)
                throw new Exceptions.ValidationException("Invalid author name");

            var topic = await context.Topics
                .FirstOrDefaultAsync(a => a.Title == command.Topic);

            if (topic == null)
                throw new Exceptions.ValidationException("Invalid topic name");

            var bookEntity = context.Books.Add(
                new Book
                {
                    ISBN = command.ISBN,
                    Title = command.Title,
                    Year = command.Year,
                    Author = author,
                    Topic = topic,
                    IsDeleted = false
                }).Entity;

            await context.SaveChangesAsync();

            return new BookResponse(
                bookEntity.ISBN,
                bookEntity.Title,
                bookEntity.Year,
                new AuthorResponse(bookEntity.Author!.Name),
                new TopicResponse(bookEntity.Topic!.Title));
        }

        public async Task DeleteBookAsync(string isbn)
        {
            var book = await context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn && !b.IsDeleted)
                ?? throw new NotFoundException();

            // Soft Delete
            book.IsDeleted = true;

            await context.SaveChangesAsync();
        }

        public async Task<BookResponse> GetBookAsync(string isbn)
        {
            var match = Regex.Match(isbn, "^\\d{13}$");

            if (!match.Success)
            {
                throw new Exceptions.ValidationException("Invalid ISBN, should be 13 digits.");
            }

            return await context.Books
                .Include(b => b.Author)
                .Include(b => b.Topic)
                .FirstOrDefaultAsync(b => b.ISBN == isbn && !b.IsDeleted)
                is Book bookEntity
                    ? new BookResponse(
                        bookEntity.ISBN,
                        bookEntity.Title,
                        bookEntity.Year,
                        new AuthorResponse(bookEntity.Author!.Name),
                        new TopicResponse(bookEntity.Topic!.Title))
                    : throw new NotFoundException();
        }

        public Task<List<BookResponse>> GetBooksAsync(SieveModel sieveModel)
        {
            var bookQuery = context.Books
                .Include(b => b.Author)
                .Include(b => b.Topic)
                .Where(b => !b.IsDeleted);

            return sieveProcessor.Apply(sieveModel, bookQuery)
                .Select(b =>
                    new BookResponse(
                        b.ISBN,
                        b.Title,
                        b.Year,
                        new AuthorResponse(b.Author!.Name),
                        new TopicResponse(b.Topic!.Title)))
                .ToListAsync();
        }

        public Task<List<BookResponse>> GetBooksByAuthorv2Async(int authorId)
        {
            return context.Books
                .Include(b => b.Author)
                .Include(b => b.Topic)
                .Where(b => !b.IsDeleted && b.AuthorId == authorId)
                .Select(b => new BookResponse(
                    b.ISBN,
                    b.Title,
                    b.Year,
                    new AuthorResponse(b.Author!.Name),
                    new TopicResponse(b.Topic!.Title)))
                .ToListAsync();
        }

        public Task<List<string>> GetBooksByAuthorAsync(int authorId)
        {
            return context.Books
                .Include(b => b.Author)
                .Include(b => b.Topic)
                .Where(b => !b.IsDeleted && b.AuthorId == authorId)
                .Select(b => 
                    b.Title)
                .ToListAsync();
        }

        public async Task<BookResponse> UpdateBookAsync(string isbn, UpdateBookCommand command)
        {
            var author = await context.Authors
                .FirstOrDefaultAsync(a => a.Name == command.Author);

            if (author == null)
                throw new Exceptions.ValidationException("Invalid author name");

            var topic = await context.Topics
                .FirstOrDefaultAsync(a => a.Title == command.Topic);

            if (topic == null)
                throw new Exceptions.ValidationException("Invalid topic name");

            var bookEntity = await context.Books
                .Include(b => b.Author)
                .Include(b => b.Topic)
                .FirstOrDefaultAsync(b => b.ISBN == isbn && !b.IsDeleted)
                ?? throw new NotFoundException();

            bookEntity.ISBN = command.ISBN;
            bookEntity.Title = command.Title;
            bookEntity.Year = command.Year;

            /* -- Non Idempotent check
             var bookEntry = context.ChangeTracker.Entries<Book>().FirstOrDefault(b => b.Entity.ISBN == isbn);
             if (bookEntry is not null && bookEntry.State == EntityState.Unchanged)
                 return Results.BadRequest(new { message = "No changes detected." });
            */

            await context.SaveChangesAsync();

            var bookResponse = new BookResponse(
                bookEntity.ISBN,
                bookEntity.Title,
                bookEntity.Year,
                new AuthorResponse(bookEntity.Author!.Name),
                new TopicResponse(bookEntity.Topic!.Title));

            return bookResponse;
        }
    }
}
