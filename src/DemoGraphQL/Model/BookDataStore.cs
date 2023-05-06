using System.Collections.Generic;

namespace DemoGraphQL.Model;

public class BookDataStore : IBookDataStore
{
    private readonly IList<Book> _books = new List<Book> {
        new Book(1, "Book 1", new Author("Author 1")),
        new Book(2, "Book 2", new Author("Author 2")),
        new Book(3, "Book 3", new Author("Author 3")),
        new Book(4, "Book 4", new Author("Author 4")),
    };

    public Book[] GetBooks() {
        return _books.ToArray();
    }

    public Book GetBook(int id)
    {
        return _books.First(b => b.Id == id);
    }

    public Book AddBook(Book book)
    {
        _books.Add(book);

        return book;
    }
}