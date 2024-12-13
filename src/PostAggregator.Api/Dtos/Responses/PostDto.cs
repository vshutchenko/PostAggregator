namespace PostAggregator.Api.Dtos.Responses;

public class PostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public string? Link { get; set; }
    public string? Thumbnail { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
