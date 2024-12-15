using FluentAssertions;
using Moq;
using PostAggregator.Api.Data.Entities;
using PostAggregator.Api.Data.Repositories.PostRepository;
using PostAggregator.Api.Data.Specification;
using PostAggregator.Api.Dtos.Requests;
using PostAggregator.Api.Exceptions;
using PostAggregator.Api.Services.PostService;
using PostAggregator.Api.Services.Reddit;

namespace PostAggregator.Test.ServicesTests;
[TestFixture]
public class PostServiceTests
{
    private Mock<IPostRepository> _mockPostRepository;
    private Mock<IRedditService> _mockRedditService;
    private PostService _postService;

    [SetUp]
    public void SetUp()
    {
        _mockPostRepository = new Mock<IPostRepository>();
        _mockRedditService = new Mock<IRedditService>();
        _postService = new PostService(_mockPostRepository.Object, _mockRedditService.Object);
    }

    [Test]
    public async Task CreatePost_ShouldCreatePost_WhenValidRequest()
    {
        // Arrange
        var createPostRequest = new CreatePostRequest
        {
            Title = "Test Title",
            Text = "Test Text",
            Author = "Test Author"
        };

        var createdPost = new Post
        {
            Id = Guid.NewGuid(),
            Title = createPostRequest.Title,
            Text = createPostRequest.Text,
            Author = createPostRequest.Author,
            CreatedAtUtc = DateTime.UtcNow,
            Source = Source.PostAggregator
        };

        _mockPostRepository.Setup(repo => repo.CreatePostAsync(It.IsAny<Post>()))
            .ReturnsAsync(createdPost);

        // Act
        var result = await _postService.CreatePost(createPostRequest);

        // Assert
        result.Should().BeEquivalentTo(createdPost);
        _mockPostRepository.Verify(repo => repo.CreatePostAsync(It.IsAny<Post>()), Times.Once);
    }

    [Test]
    public async Task GetPostByIdAsync_ShouldReturnPost_WhenPostExists()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var expectedPost = new Post
        {
            Id = postId,
            Title = "Test Title",
            Text = "Test Text",
            Author = "Test Author",
            CreatedAtUtc = DateTime.UtcNow,
            Source = Source.PostAggregator
        };

        _mockPostRepository.Setup(repo => repo.GetPostByIdAsync(postId))
            .ReturnsAsync(expectedPost);

        // Act
        var result = await _postService.GetPostByIdAsync(postId);

        // Assert
        result.Should().BeEquivalentTo(expectedPost);
        _mockPostRepository.Verify(repo => repo.GetPostByIdAsync(postId), Times.Once);
    }

    [Test]
    public async Task GetPostByIdAsync_ShouldThrowNotFoundException_WhenPostDoesNotExist()
    {
        // Arrange
        var postId = Guid.NewGuid();

        _mockPostRepository.Setup(repo => repo.GetPostByIdAsync(postId))
            .ThrowsAsync(new NotFoundException("Post not found."));         

        // Act
        var act = async () => await _postService.GetPostByIdAsync(postId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _mockPostRepository.Verify(repo => repo.GetPostByIdAsync(postId), Times.Once);
    }

    [Test]
    public async Task GetPostsAsync_ShouldReturnPostsFromRepository_WhenPostsExist()
    {
        // Arrange
        var pageRequest = new PageRequest { Page = 1, PageSize = 10, OrderColumn = "Title", Asc = true };
        var postList = new List<Post>
        {
            new Post { Id = Guid.NewGuid(), Title = "Post 1" },
            new Post { Id = Guid.NewGuid(), Title = "Post 2" }
        };

        _mockPostRepository.Setup(repo => repo.GetPostsCountAsync()).ReturnsAsync(2);
        _mockPostRepository.Setup(repo => repo.GetPostsAsync(It.IsAny<ISpecification>()))
            .ReturnsAsync(postList);

        // Act
        var result = await _postService.GetPostsAsync(pageRequest);

        // Assert
        result.Should().BeEquivalentTo(postList);
        _mockPostRepository.Verify(repo => repo.GetPostsAsync(It.IsAny<ISpecification>()), Times.Once);
    }

    [Test]
    public async Task GetPostsAsync_ShouldFetchPostsFromReddit_WhenNoPostsExist()
    {
        // Arrange
        var pageRequest = new PageRequest { Page = 1, PageSize = 10, OrderColumn = "Title", Asc = true };
        var redditPosts = new List<Post>
        {
            new Post { Id = Guid.NewGuid(), Title = "Reddit Post 1" },
            new Post { Id = Guid.NewGuid(), Title = "Reddit Post 2" }
        };

        _mockPostRepository.Setup(repo => repo.GetPostsCountAsync()).ReturnsAsync(0);
        _mockRedditService.Setup(service => service.GetPostsAsync()).ReturnsAsync(redditPosts);
        _mockPostRepository.Setup(repo => repo.CreatePostAsync(It.IsAny<Post>()))
            .ReturnsAsync((Post post) => new Post
            {
                Id = Guid.NewGuid(),
                Title = post.Title,
                Text = post.Text,
                Author = post.Author,
                CreatedAtUtc = post.CreatedAtUtc,
                Source = post.Source
            });

        // Act
        var result = await _postService.GetPostsAsync(pageRequest);

        // Assert
        _mockRedditService.Verify(service => service.GetPostsAsync(), Times.Once);
        _mockPostRepository.Verify(repo => repo.CreatePostAsync(It.IsAny<Post>()), Times.Exactly(redditPosts.Count));
    }
}