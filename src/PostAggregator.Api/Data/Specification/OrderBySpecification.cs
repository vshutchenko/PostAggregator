namespace PostAggregator.Api.Data.Specification;

public class OrderBySpecification : ISpecification
{
    private const string ParameterName = "@columnName";

    public OrderBySpecification(string columnName, bool asc)
    {
        ColumnName = columnName;
        Asc = asc;
    }

    public string ColumnName { get; }
    public bool Asc { get; }

    public Dictionary<string, object> GetParameters()
    {
        return new Dictionary<string, object>() { { ParameterName, ColumnName } };
    }

    public string GetSqlQuery()
    {
        var clause = $"{Environment.NewLine}ORDER BY {ParameterName}";
        if (Asc)
        {
            clause += " DESC";
        }
        
        return clause ;
    }
}
