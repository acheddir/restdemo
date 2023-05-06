namespace DemoGraphQL.Model;

public interface IBookDataStore
{
    Book[] GetBooks();
    Book GetBook(int id);

    Book AddBook(Book book);
}