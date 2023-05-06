using DemoGraphQL.Dto;
using DemoGraphQL.GraphQL.Types;
using DemoGraphQL.Model;
using GraphQL;
using GraphQL.Types;

namespace DemoGraphQL.GraphQL;

public class BookMutation : ObjectGraphType
{
    public BookMutation(IBookDataStore dataStore)
    {
        Field<BookType>("addBook")
            .Argument<NonNullGraphType<BookInputType>>("input")
            .Resolve(ctx => {
                var input = ctx.GetArgument<BookInput>("input");

                var book = new Book(input.Id, input.Title, new Author(input.AuthorName));
                dataStore.AddBook(book);

                return book;
            });
    }
}