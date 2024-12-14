using PostAggregator.Api.Data.Entities;

namespace PostAggregator.Api.Services.Reddit;

public interface IRedditService
{
    public Task<IEnumerable<Post>> GetPostsAsync();
}
