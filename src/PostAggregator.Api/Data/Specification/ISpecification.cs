namespace PostAggregator.Api.Data.Specification;

public interface ISpecification
{
    public string GetSqlQuery();

    public Dictionary<string, object> GetParameters();
}
