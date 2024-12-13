namespace PostAggregator.Api.Data.Specification;

public class Specification : ISpecification
{
    private List<ISpecification> _specifications = new();

    private ISpecification? OrderBySpecification => 
        _specifications.FirstOrDefault(s => s is OrderBySpecification);
    private ISpecification? LimitOffsetSpecification => 
        _specifications.FirstOrDefault(s => s is LimitOffsetSpecification);

    public void AddSpecification(ISpecification specification)
    {
        _specifications.Add(specification);
    }

    public Dictionary<string, object> GetParameters()
    {
        var result = new Dictionary<string, object>();

        foreach (var specification in _specifications)
        {
            foreach (var param in specification.GetParameters())
            {
                result.Add(param.Key, param.Value);
            }
        }

        return result;
    }

    public string GetSqlQuery()
    {
        var commandText =
            $"{Environment.NewLine}SELECT * from Post" +
            $" {OrderBySpecification?.GetSqlQuery()}" +
            $" {LimitOffsetSpecification?.GetSqlQuery()}";

        return commandText;
    }
}