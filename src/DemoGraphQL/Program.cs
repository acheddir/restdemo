using DemoGraphQL.GraphQL;
using DemoGraphQL.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IBookDataStore, BookDataStore>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<BookQuery>()
    .AddMutationType<BookMutation>()
    .AddSubscriptionType<BookSubscription>()
    .AddMutationConventions()
    .AddInMemorySubscriptions();

var app = builder.Build();

app.UseWebSockets();
app.MapGraphQL();

app.Run();
