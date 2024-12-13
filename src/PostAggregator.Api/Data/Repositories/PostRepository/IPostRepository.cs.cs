using PostAggregator.Api.Data.Entities;
using PostAggregator.Api.Data.Specification;

namespace PostAggregator.Api.Data.Repositories.PostRepository;

public interface IPostRepository
{
    public Task<IEnumerable<Post>> GetPostsAsync(ISpecification specification);
    public Task<Post> GetPostByIdAsync(Guid id);
    public Task<Post> CreatePostAsync(Post post);
}
