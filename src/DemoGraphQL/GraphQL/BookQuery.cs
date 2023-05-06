using DemoGraphQL.GraphQL.Types;
using DemoGraphQL.Model;
using GraphQL;
using GraphQL.Types;

namespace DemoGraphQL.GraphQL;

public class BookQuery : ObjectGraphType
{
    public BookQuery(IBookDataStore dataStore)
    {
        Field<ListGraphType<BookType>>("books").Resolve(ctx => dataStore.GetBooks());
        
        Field<BookType>("book")
            .Argument<NonNullGraphType<IntGraphType>>("id")
            .Resolve(ctx =>dataStore.GetBook(ctx.GetArgument<int>("id")));
    }
}