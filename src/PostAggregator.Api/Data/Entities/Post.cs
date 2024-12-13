namespace PostAggregator.Api.Data.Entities;

public class Post
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public string? Link { get; set; }
    public string? Thumbnail { get; set; }
    public Source Source { get; set; }
    public string? Text { get; set; }
}
