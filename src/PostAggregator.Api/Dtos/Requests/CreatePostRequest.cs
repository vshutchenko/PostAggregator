namespace PostAggregator.Api.Dtos.Requests;

/// <summary>
/// Represents the request model for creating a new post.
/// </summary>
public class CreatePostRequest
{
    /// <summary>
    /// The title of the post.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The author of the post.
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// The main text content of the post.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// The thumbnail of the post in base64 format.
    /// </summary>
    public string? Thumbnail { get; set; }
}
