namespace PostAggregator.Api.Dtos.Requests;

/// <summary>
/// Represents a request for paginated data when retrieving a list of posts.
/// </summary>
public class PageRequest
{
    /// <summary>
    /// The page number to retrieve. Must be greater than 0.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// The number of posts per page. Must be greater than 0.
    /// </summary>
    public int PageSize { get; set; } = 9;

    /// <summary>
    /// The column by which to order the results. Valid values are the property names of the post (case-insensitive).
    /// </summary>
    public string OrderColumn { get; set; } = "author";

    /// <summary>
    /// Indicates whether the sorting should be in ascending order.
    /// </summary>
    public bool Asc { get; set; }
}
