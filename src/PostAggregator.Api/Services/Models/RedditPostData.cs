using Newtonsoft.Json;

namespace PostAggregator.Api.Services.Models;

public class RedditPostData
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Thumbnail { get; set; } = string.Empty;
    [JsonProperty("created_utc")]
    public long CreatedUtc { get; set; }
    [JsonProperty("selftext")]
    public string? Text { get; set; }
}
