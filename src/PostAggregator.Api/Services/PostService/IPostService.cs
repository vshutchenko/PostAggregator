using PostAggregator.Api.Data.Entities;
using PostAggregator.Api.Dtos.Requests;

namespace PostAggregator.Api.Services.PostService;

public interface IPostService
{
    public Task<IEnumerable<Post>> GetPostsAsync(PageRequest pageRequest);
    public Task<Post> GetPostByIdAsync(Guid id);
    public Task<Post> CreatePost(CreatePostRequest createPostRequest);
}
