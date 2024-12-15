namespace PostAggregator.Api.Infrastructure;

public abstract class BaseEnvironmentHelper
{
    public const string RedditUsername = "REDDIT_USERNAME";
    public const string RedditPassword = "REDDIT_PASSWORD";
    public const string RedditClientId = "REDDIT_CLIENT_ID";
    public const string RedditClientSecret = "REDDIT_CLIENT_SECRET";

    public const string ConnectionString = "CONNECTION_STRING";

    public readonly static string[] RequiredVariables = [RedditClientId, RedditClientSecret, RedditPassword, RedditUsername, ConnectionString];

    public abstract void EnsureRequiredVariablesSet();
    public abstract string GetRequiredVariable(string variableName);
}
