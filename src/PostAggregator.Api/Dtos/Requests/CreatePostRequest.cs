namespace PostAggregator.Api.Dtos.Requests;

public class CreatePostRequest
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
