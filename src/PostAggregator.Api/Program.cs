using Serilog;
using PostAggregator.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddApplication();
builder.Services.AddCache();
builder.Services.AddCorsPolicy();
builder.Services.AddSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.EnsureRequiredVariablesSet();
app.InitializeDb();

app.UseCors("AllowAll");

app.UseOutputCache();

app.UseExceptionHandler();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
