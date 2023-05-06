using DemoGraphQL.Model;

namespace DemoGraphQL.GraphQL;

public class BookQuery
{
    private readonly IBookDataStore dataStore;

    public BookQuery(IBookDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public Book[] GetBooks()
    {
        return dataStore.GetBooks();
    }
}