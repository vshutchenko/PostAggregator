using PostAggregator.Api.Data;
using PostAggregator.Api.Data.Repositories.PostRepository;
using PostAggregator.Api.Infrastructure;
using PostAggregator.Api.Services.PostService;
using Serilog;
using FluentValidation;
using PostAggregator.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<CreatePostRequestValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

var db = new DbInitializer();
db.Initialize();

if (app.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
    db.AddDemoData();
    app.UseSwagger();
    app.UseSwaggerUI();
}

EnvironmentHelper.EnsureRequiredVariablesSet();

app.UseExceptionHandler();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
