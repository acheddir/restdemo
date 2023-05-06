using DemoGraphQL.GraphQL;
using DemoGraphQL.GraphQL.Types;
using DemoGraphQL.Model;
using GraphQL;
using GraphQL.Types;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IBookDataStore, BookDataStore>();

builder.Services.AddSingleton<BookType>();
builder.Services.AddSingleton<BookQuery>();
builder.Services.AddSingleton<BookMutation>();
builder.Services.AddSingleton<ISchema, BookSchema>();

builder.Services.AddGraphQL(options =>
    options
        .AddAutoSchema<BookSchema>()
        .AddSystemTextJson());

var app = builder.Build();

app.UseGraphQL<ISchema>("/graphql");
app.UseGraphQLGraphiQL();
app.UseGraphQLAltair();
app.UseGraphQLPlayground();
app.UseGraphQLVoyager();

app.Run();
