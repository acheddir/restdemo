using DemoGraphQL.Model;
using HotChocolate.Subscriptions;

namespace DemoGraphQL.GraphQL;

public class BookMutation
{
    private readonly IBookDataStore dataStore;
    private readonly ITopicEventSender eventSender;

    public BookMutation(IBookDataStore dataStore, ITopicEventSender eventSender)
    {
        this.dataStore = dataStore;
        this.eventSender = eventSender;
    }

    public async Task<Book> PublishBook(int id, string title, string authorName,
        CancellationToken cancellationToken)
    {
        var book = new Book(id, title, new Author(authorName));

        dataStore.AddBook(book);

        await eventSender.SendAsync(nameof(PublishBook), book, cancellationToken);

        return book;
    }
}