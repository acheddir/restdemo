using GraphQL.Types;

namespace DemoGraphQL.GraphQL.Types;

public class BookInputType : InputObjectGraphType
{
    public BookInputType()
    {
        Name = "BookInput";
        Field<NonNullGraphType<IntGraphType>>("id");
        Field<StringGraphType>("title");
        Field<StringGraphType>("authorName");
    }
}