using FluentValidation;
using PostAggregator.Api.Data;
using PostAggregator.Api.Data.Repositories.PostRepository;
using PostAggregator.Api.Infrastructure;
using PostAggregator.Api.Services.PostService;
using PostAggregator.Api.Services.Reddit;
using PostAggregator.Api.Validators;
using System.Reflection;

namespace PostAggregator.Api.Extensions;

public static class ApplicationExtensions
{
    public static void EnsureRequiredVariablesSet(this WebApplication app)
    {
        var environmentHelper = app.Services.GetRequiredService<BaseEnvironmentHelper>();
        environmentHelper.EnsureRequiredVariablesSet();
    }

    public static void InitializeDb(this WebApplication app)
    {
        var environmentHelper = app.Services.GetRequiredService<BaseEnvironmentHelper>();
        var db = new DbInitializer(environmentHelper.GetRequiredVariable(BaseEnvironmentHelper.ConnectionString));
        db.Initialize();
    }
}
