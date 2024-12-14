namespace PostAggregator.Api.Services.Models;

public class RedditPostData
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Selftext { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public long CreatedUtc { get; set; }
}
