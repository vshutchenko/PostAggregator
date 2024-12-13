using PostAggregator.Api.Data.Entities;
using PostAggregator.Api.Data.Specification;

namespace PostAggregator.Api.Data.Repositories.PostRepository;

public interface IPostRepository
{
    public Task<IEnumerable<PostEntity>> GetPostsAsync(ISpecification specification);
    public Task<PostEntity> GetPostByIdAsync(Guid id);
    public Task<PostEntity> CreatePostAsync(PostEntity post);
}
