namespace PostAggregator.Api.Dtos.Responses;

/// <summary>
/// Represents the Data Transfer Object (DTO) for a post. This is used to return post data in API responses.
/// </summary>
public class PostDto
{
    /// <summary>
    /// The unique identifier of the post.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The title of the post.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The author of the post.
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// The date and time when the post was created, in UTC format.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// The URL link to the post. This is optional and may be null.
    /// </summary>
    public string? Link { get; set; }

    /// <summary>
    /// The URL for the post's thumbnail image. This is optional and may be null.
    /// </summary>
    public string? Thumbnail { get; set; }

    /// <summary>
    /// The source of the post (e.g., Reddit, personal blog, etc.).
    /// </summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// The main text content of the post.
    /// </summary>
    public string Text { get; set; } = string.Empty;
}
