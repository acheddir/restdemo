using GraphQL.Types;

namespace DemoGraphQL.GraphQL;

public class BookSchema : Schema
{
    public BookSchema(IServiceProvider serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<BookQuery>();
        Mutation = serviceProvider.GetRequiredService<BookMutation>();
    }
}