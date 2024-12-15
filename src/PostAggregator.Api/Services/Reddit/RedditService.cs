using AutoMapper;
using Newtonsoft.Json;
using PostAggregator.Api.Data.Entities;
using PostAggregator.Api.Infrastructure;
using PostAggregator.Api.Services.Models;
using System.Net.Http.Headers;

namespace PostAggregator.Api.Services.Reddit;

public class RedditService : IRedditService
{
    private readonly string RedditUsername;
    private readonly string RedditPassword;
    private readonly string RedditClientId;
    private readonly string RedditClientSecret;

    private ILogger<RedditService> _logger;
    private IMapper _mapper;
    private IHttpClientFactory _clientFactory;

    public RedditService(
        IMapper mapper,
        ILogger<RedditService> logger,     
        IHttpClientFactory clientFactory,
        BaseEnvironmentHelper environmentHelper)
    {
        _logger = logger;
        _mapper = mapper;
        _clientFactory = clientFactory;

        RedditUsername = environmentHelper.GetRequiredVariable(BaseEnvironmentHelper.RedditUsername);
        RedditPassword = environmentHelper.GetRequiredVariable(BaseEnvironmentHelper.RedditPassword);
        RedditClientId = environmentHelper.GetRequiredVariable(BaseEnvironmentHelper.RedditClientId);
        RedditClientSecret = environmentHelper.GetRequiredVariable(BaseEnvironmentHelper.RedditClientSecret);
    }

    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
        using var client = _clientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("User-Agent", "PostAggregator");
        client.BaseAddress = new Uri("https://www.reddit.com/");

        try
        {
            string? accessToken = await GetAccessToken(client);
            var posts = await GetRedditPosts(client, accessToken!);
            _logger.LogInformation("Fetched {count} posts from reddit.", posts.Count());
            return posts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured during fetching posts from reddit.");
            return Enumerable.Empty<Post>();
        }
    }

    private async Task<string?> GetAccessToken(HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{RedditClientId}:{RedditClientSecret}"))
        );

        var content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("username", RedditUsername),
            new KeyValuePair<string, string>("password", RedditPassword)
        ]);

        var response = await client.PostAsync("/api/v1/access_token", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            _logger.LogError(errorResponse);
            return null;
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        var accessTokenResponse = JsonConvert.DeserializeObject<RedditAccessTokenResponse>(responseBody)!;

        return accessTokenResponse?.AccessToken;
    }

    private async Task<IEnumerable<Post>> GetRedditPosts(HttpClient client, string accessToken, int limit = 50)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.GetAsync($"/r/funny/top.json?limit={limit}");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        var redditPostResponse = JsonConvert.DeserializeObject<RedditPostResponse>(responseBody)!;

        var posts = _mapper.Map<List<Post>>(redditPostResponse.Data.Children.Select(x => x.Data).ToList());

        return posts;
    }
}
