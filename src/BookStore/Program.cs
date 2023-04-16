using BookStore.Filters;
using BookStore.Services;
using BookStore.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Sieve.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BookStore")));

builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
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

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(config =>
{
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

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
    //config.ApiVersionReader = ApiVersionReader.Combine(
    //    //new QueryStringApiVersionReader("api-version"),
    //    //new HeaderApiVersionReader("X-Version")
    //    //new MediaTypeApiVersionReader("version")
    //);
});

builder.Services.AddVersionedApiExplorer(config =>
{
    config.GroupNameFormat = "'v'VVV";
    config.SubstituteApiVersionInUrl = true;
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddControllers(config => config.Filters.Add<ErrorHandlerFilterAttribute>());

builder.Services.AddValidatorsFromAssemblyContaining<CreateBookCommandValidator>();

builder.Services.AddScoped<SieveProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            config.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName);
        }

        config.OAuthClientId(builder.Configuration["AzureAd:Audience"]);
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    readonly IApiVersionDescriptionProvider provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
    this.provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Version = description.ApiVersion.ToString(),
                Title = $"API v{description.ApiVersion}",
            });
        }
    }
}