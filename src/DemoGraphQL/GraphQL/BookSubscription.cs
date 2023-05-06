using DemoGraphQL.Model;

namespace DemoGraphQL.GraphQL;

public class BookSubscription
{
    [Subscribe]
    [Topic(nameof(BookMutation.PublishBook))]
    public Book OnPublished([EventMessage] Book publishedBook) => publishedBook;
}