using DemoGraphQL.Model;
using GraphQL.Types;

namespace DemoGraphQL.GraphQL.Types;

public class AuthorType : ObjectGraphType<Author>
{
    public AuthorType()
    {
        Field(x => x.Name);
    }
}