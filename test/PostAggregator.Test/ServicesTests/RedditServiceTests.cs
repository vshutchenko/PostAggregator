using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Microsoft.Extensions.Logging;
using AutoMapper;
using PostAggregator.Api.Data.Entities;
using PostAggregator.Api.Infrastructure;
using PostAggregator.Api.Services.Models;
using PostAggregator.Api.Services.Reddit;

namespace PostAggregator.Test.RedditServiceTests;

[TestFixture]
public class RedditServiceTests
{
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<RedditService>> _mockLogger;
    private Mock<BaseEnvironmentHelper> _mockEnvironmentHelper;
    private Mock<IHttpClientFactory> _mockHttpClientFactory;
    private RedditService _redditService;
    private MockHttpMessageHandler _mockHttpMessageHandler;
    private HttpClient _httpClient;

    [SetUp]
    public void SetUp()
    {
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<RedditService>>();
        _mockEnvironmentHelper = new Mock<BaseEnvironmentHelper>();

        _mockEnvironmentHelper.Setup(helper => helper.GetRequiredVariable(It.IsAny<string>()))
            .Returns("test");

        _mockHttpMessageHandler = new MockHttpMessageHandler();

        _httpClient = _mockHttpMessageHandler.ToHttpClient();

        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(_httpClient);

        _redditService = new RedditService(
            _mockMapper.Object,
            _mockLogger.Object,
            _mockHttpClientFactory.Object,
            _mockEnvironmentHelper.Object);
    }

    [Test]
    public async Task GetPostsAsync_ShouldReturnPosts_WhenSuccessful()
    {
        // Arrange
        var redditPostsResponse = new RedditPostResponse
        {
            Data = new RedditData
            {
                Children = new List<RedditPost>
                {
                    new RedditPost
                    {
                        Data = new RedditPostData
                        {
                            Title = "Post 1",
                            Author = "Author 1",
                            Url = "https://example.com/post1",
                            Thumbnail = "https://example.com/thumb1",
                            CreatedUtc = 1234567890
                        }
                    }
                }
            }
        };

        _mockHttpMessageHandler
            .When("https://www.reddit.com/api/v1/access_token")
            .Respond("application/json", "{\"access_token\":\"mockAccessToken\",\"token_type\":\"bearer\"}");

        _mockHttpMessageHandler
            .When("https://www.reddit.com/r/funny/top.json?limit=50")
            .Respond("application/json", JsonConvert.SerializeObject(redditPostsResponse));

        _mockMapper.Setup(mapper => mapper.Map<List<Post>>(It.IsAny<List<RedditPostData>>()))
            .Returns(new List<Post> { new Post { Title = "Post 1" } });

        // Act
        var posts = await _redditService.GetPostsAsync();

        // Assert
        posts.Count().Should().Be(1);
        posts.First().Title.Should().Be("Post 1");
    }

    [Test]
    public async Task GetPostsAsync_ShouldReturnEmptyList_WhenAccessTokenIsNull()
    {
        // Arrange
        _mockHttpMessageHandler
            .When("https://www.reddit.com/api/v1/access_token")
            .Respond("application/json", "{\"error\":\"invalid_request\"}");

        // Act
        var posts = await _redditService.GetPostsAsync();

        // Assert
        posts.Should().BeEmpty();
    }

    [Test]
    public async Task GetPostsAsync_ShouldReturnEmptyList_WhenRedditPostsRequestFails()
    {
        // Arrange
        _mockHttpMessageHandler
            .When("https://www.reddit.com/api/v1/access_token")
            .Respond("application/json", "{\"access_token\":\"mockAccessToken\",\"token_type\":\"bearer\"}");

        _mockHttpMessageHandler
            .When("https://www.reddit.com/r/funny/top.json?limit=50")
            .Respond(System.Net.HttpStatusCode.BadRequest);

        // Act
        var posts = await _redditService.GetPostsAsync();

        // Assert
        posts.Should().BeEmpty();
    }

    [TearDown]
    public void TearDown()
    {
        _mockHttpMessageHandler.Dispose();
        _httpClient.Dispose();
    }
}
