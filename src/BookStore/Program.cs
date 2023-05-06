using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("BookStore"));
dataSourceBuilder.MapEnum<BookStatus>();

builder.Services.AddDbContext<BookStoreContext>(options => options.UseNpgsql(dataSourceBuilder.Build()));

builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["AzureAd:Authority"];
    options.Audience = builder.Configuration["AzureAd:Audience"];
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();

            c.Response.StatusCode = 401;
            c.Response.ContentType = "text/plain";

            return c.Response.WriteAsync("An error occured while authenticating");
        }
    };
});

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
    config.Policies.Sunset(1.0)
        .Effective(DateTimeOffset.Now.AddDays(60))
        .Link("policy.html")
            .Title("Versioning Policy")
            .Type("text/html");
    //config.ApiVersionReader = ApiVersionReader.Combine(
    //    //new QueryStringApiVersionReader("api-version"),
    //    //new HeaderApiVersionReader("X-Version")
    //    //new MediaTypeApiVersionReader("version")
    //);
})
.AddApiExplorer(config =>
{
    config.GroupNameFormat = "'v'VVV";
    config.SubstituteApiVersionInUrl = true;
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.OperationFilter<SwaggerDefaultValues>();
    config.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{builder.Configuration.GetValue<string>("AzureAd:Authority")}/oauth2/v2.0/authorize"),
                TokenUrl = new Uri($"{builder.Configuration.GetValue<string>("AzureAd:Authority")}/oauth2/v2.0/token"),
                Scopes = new Dictionary<string, string>
                {
                    {
                        $"{builder.Configuration["AzureAd:Audience"]}/Books.Read",
                        "Read your books"
                    }
                }
            }
        }
    });
    config.OperationFilter<AuthorizeCheckOperationFilter>();
    config.EnableAnnotations();
});

builder.Services.AddControllers(config => config.Filters.Add<ErrorHandlerFilterAttribute>());
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookCommandValidator>();

builder.Services.AddScoped<SieveProcessor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        var descriptions = app.DescribeApiVersions();

        // build a swagger endpoint for each discovered API version
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            config.SwaggerEndpoint(url, name);
        }

        config.OAuthClientId(builder.Configuration["AzureAd:Audience"]);
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();