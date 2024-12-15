namespace PostAggregator.Api.Infrastructure;

public class EnvironmentHelper : BaseEnvironmentHelper
{
    public override void EnsureRequiredVariablesSet()
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

    public override string GetRequiredVariable(string variableName)
    {
        return Environment.GetEnvironmentVariable(variableName) ??
            throw new InvalidOperationException(
                $"Environment variable is missing({variableName}).");
    }
}
