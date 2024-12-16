using FluentValidation;
using PostAggregator.Api.Data.Repositories.PostRepository;
using PostAggregator.Api.Infrastructure;
using PostAggregator.Api.Services.PostService;
using PostAggregator.Api.Services.Reddit;
using PostAggregator.Api.Validators;
using System.Reflection;

namespace PostAggregator.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IRedditService, RedditService>();
        services.AddScoped<IPostService, PostService>();

        services.AddControllers();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssemblyContaining<CreatePostRequestValidator>();

        services.AddTransient<BaseEnvironmentHelper, EnvironmentHelper>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddHttpClient();
    }

    public static void AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });
    }

    public static void AddCache(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.AddPolicy("PostsPolicy", policy =>
                policy.Expire(TimeSpan.FromSeconds(60)).Tag("posts"));
        });
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }
}
