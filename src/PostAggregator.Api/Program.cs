using PostAggregator.Api.Data;
using PostAggregator.Api.Data.Repositories.PostRepository;
using PostAggregator.Api.Infrastructure;
using PostAggregator.Api.Services.PostService;
using Serilog;
using FluentValidation;
using PostAggregator.Api.Validators;
using PostAggregator.Api.Services.Reddit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<BaseEnvironmentHelper, EnvironmentHelper>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IRedditService, RedditService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddValidatorsFromAssemblyContaining<CreatePostRequestValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("PostsPolicy", policy =>
        policy.Expire(TimeSpan.FromSeconds(60)).Tag("posts"));
});

builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var db = new DbInitializer();
    db.Initialize();
    DotNetEnv.Env.Load();
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var environmentHelper = scope.ServiceProvider.GetRequiredService<BaseEnvironmentHelper>();
    environmentHelper.EnsureRequiredVariablesSet();
}

app.UseOutputCache();

app.UseExceptionHandler();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
