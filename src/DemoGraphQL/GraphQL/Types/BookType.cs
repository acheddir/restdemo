using DemoGraphQL.Model;
using GraphQL.Types;

namespace DemoGraphQL.GraphQL.Types;

public class BookType : ObjectGraphType<Book>
{
    public BookType()
    {
        Field(x => x.Id);
        Field(x => x.Title);
        Field<AuthorType>("author").Resolve(ctx => ctx.Source.Author);
    }
}