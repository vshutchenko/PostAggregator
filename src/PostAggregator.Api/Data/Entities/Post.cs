namespace PostAggregator.Api.Data.Entities;

public class PostEntity
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string CreatedAtUtc { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string Thumbnail { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string? Text { get; set; }
}
