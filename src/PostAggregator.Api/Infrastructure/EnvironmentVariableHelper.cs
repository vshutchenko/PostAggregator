namespace PostAggregator.Api.Infrastructure;

public static class EnvironmentVariableHelper
{
    public const string RedditUsername = "REDDIT_USERNAME";
    public const string RedditPassword = "REDDIT_PASSWORD";
    public const string RedditClientId = "REDDIT_CLIENT_ID";
    public const string RedditClientSecret = "REDDIT_CLIENT_SECRET";

    public const string ConnectionString = "CONNECTION_STRING";

    public readonly static string[] RequiredVariables = [RedditClientId, RedditClientSecret, RedditPassword, RedditUsername, ConnectionString];

    public static void EnsureRequiredVariablesSet()
    {
        var notSetVariables = new List<string>();

        foreach (var envVar in RequiredVariables)
        {
            var value = Environment.GetEnvironmentVariable(envVar);
            if (string.IsNullOrEmpty(value))
            {
                notSetVariables.Add(envVar);
            }
        }

        if (notSetVariables.Any())
        {
            throw new InvalidOperationException(
                $"One or more required environment variables are missing(" +
                $"{string.Join(", ", notSetVariables)}). Application cannot start.");
        }
    }

    public static string GetVariable(string variableName)
    {
        return Environment.GetEnvironmentVariable(variableName) ??
            throw new InvalidOperationException(
                $"Environment variable is missing({variableName}).");
    }
}
