using DemoGraphQL.GraphQL;
using DemoGraphQL.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IBookDataStore, BookDataStore>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowedCors", builder =>
        builder
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services
    .AddGraphQLServer()
    .AddQueryType<BookQuery>()
    .AddMutationType<BookMutation>()
    .AddSubscriptionType<BookSubscription>()
    .AddMutationConventions()
    .AddInMemorySubscriptions();

var app = builder.Build();

app.UseCors("AllowedCors");

app.UseWebSockets();

app.MapGraphQL();

app.Run();
