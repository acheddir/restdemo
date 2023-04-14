var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BookStore")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/v1/books", async (BookStoreContext context) =>
{
    var books = await context.Books
        .Where(b => !b.IsDeleted)
        .Select(b => new BookResponse(b.ISBN, b.Title, b.Year))
        .ToListAsync();

    return Results.Ok(books);
});

app.MapGet("api/v1/books/{isbn}", async (BookStoreContext context, string isbn) =>
    await context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn && !b.IsDeleted)
        is Book bookEntity
            ? Results.Ok(new BookResponse(bookEntity.ISBN, bookEntity.Title, bookEntity.Year))
            : Results.NotFound());

app.MapPost("api/v1/books", async (BookStoreContext context, CreateBookCommand command) =>
{
    var bookEntity = context.Books.Add(new Book { ISBN = command.ISBN, Title = command.Title, Year = command.Year, IsDeleted = false }).Entity;

    await context.SaveChangesAsync();

    var bookResponse = new BookResponse(bookEntity.ISBN, bookEntity.Title, bookEntity.Year);

    return Results.Created($"api/v1/books/{bookResponse.ISBN}", bookResponse);
});

app.MapDelete("api/v1/books/{isbn}", async (BookStoreContext context, string isbn) =>
{
    if (await context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn && !b.IsDeleted) is Book book)
    {
        // Soft Delete
        book.IsDeleted = true;

        await context.SaveChangesAsync();
        
        return Results.Ok();
    }

    return Results.NotFound();
});

app.MapPut("api/v1/books/{isbn}", async (BookStoreContext context, string isbn, UpdateBookCommand command) =>
{
    var bookEntity = await context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn && !b.IsDeleted);

    if (bookEntity is null)
        return Results.NotFound();

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

    return Results.Ok(bookResponse);
});

app.Run();