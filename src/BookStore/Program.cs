using BookStore.Services;
using BookStore.Services.Dto;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BookStore")));

//builder.Services.AddAutoMapper(typeof(BookProfile).Assembly);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IBookService, BookService>();

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

app.MapGet("api/v1/books", async (IBookService bookService) =>
{
    var books = await bookService.GetBooksAsync();
    return Results.Ok(books);
});

app.MapGet("api/v1/books/{id}", async (IBookService bookService, int id) =>
{
    var book = await bookService.GetBookById(id);

    if (book is null)
        return Results.NotFound();

    return Results.Ok(book);
});

app.MapPost("api/v1/books", async (IBookService bookService, CreateBookCommand command) =>
{
    var bookResponse = await bookService.CreateBook(command);
    return Results.Created($"api/v1/books/{bookResponse.Title}", bookResponse);
});

app.MapDelete("api/v1/books/{id}", async (IBookService bookService, int id) =>
{
    var result = await bookService.RemoveBook(id);

    if (result <= 0)
        return Results.NotFound();

    return Results.Ok();
});

app.MapPut("api/v1/books/{id}", async (IBookService bookService, int id, UpdateBookCommand command) =>
{
    var bookResponse = await bookService.UpdateBook(id, command);

    if (bookResponse is null)
        return Results.NotFound();

    return Results.Ok(bookResponse);
});

app.Run();