namespace PostAggregator.Api.Dtos.Requests;

public class PageRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
    public string OrderColumn { get; set; } = "author";
    public bool Asc { get; set; }
}
