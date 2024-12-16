using System;

namespace PostAggregator.Api.Data.Specification;

public class OrderBySpecification : ISpecification
{
    public OrderBySpecification(string columnName, bool asc)
    {
        ColumnName = columnName;
        Asc = asc;
    }

    public string ColumnName { get; }
    public bool Asc { get; }

    public Dictionary<string, object> GetParameters()
    {
        // SQLite does not support passing column name using parameter syntax.
        // Column name passed as a part of a query.
        return new Dictionary<string, object>();
    }

    public string GetSqlQuery()
    {
        var clause = string.Empty;

        if (ColumnName.Equals("createdatutc", StringComparison.OrdinalIgnoreCase))
        {
            clause = $"{Environment.NewLine}ORDER BY datetime({ColumnName})";
        }
        else
        {
            clause = $"{Environment.NewLine}ORDER BY {ColumnName}";
        }

        if (!Asc)
        {
            clause += " DESC";
        }
        
        return clause ;
    }
}
