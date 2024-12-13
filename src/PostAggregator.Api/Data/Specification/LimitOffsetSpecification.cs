namespace PostAggregator.Api.Data.Specification;

public class LimitOffsetSpecification : ISpecification
{
    private const string LimitParameterName = "@limit";
    private const string OffsetParameterName = "@offset";

    public LimitOffsetSpecification(int page, int pageSize)
    {
        Limit = pageSize;
        Offset = (page - 1) * pageSize;
    }

    public int Limit { get; }
    public int Offset { get; }

    public string GetSqlQuery()
    {
        return $"{Environment.NewLine}LIMIT {LimitParameterName} OFFSET {OffsetParameterName}";
    }

    public Dictionary<string, object> GetParameters()
    {
        return new Dictionary<string, object>
        {
            { LimitParameterName, Limit },
            { OffsetParameterName, Offset },
        };
    }
}
