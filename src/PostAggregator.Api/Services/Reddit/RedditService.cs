using AutoMapper;
using Newtonsoft.Json;
using PostAggregator.Api.Data.Entities;
using PostAggregator.Api.Infrastructure;
using PostAggregator.Api.Services.Models;
using System.Net.Http.Headers;

namespace PostAggregator.Api.Services.Reddit;

public class RedditService : IRedditService
{
    private const string UserAgent = "PostAggregator";
    private readonly string RedditUsername = EnvironmentVariableHelper.GetVariable(EnvironmentVariableHelper.RedditUsername);
    private readonly string RedditPassword = EnvironmentVariableHelper.GetVariable(EnvironmentVariableHelper.RedditPassword);
    private readonly string RedditClientId = EnvironmentVariableHelper.GetVariable(EnvironmentVariableHelper.RedditClientId);
    private readonly string RedditClientSecret = EnvironmentVariableHelper.GetVariable(EnvironmentVariableHelper.RedditClientSecret);

    private ILogger<RedditService> _logger;
    private IMapper _mapper;

    public RedditService(ILogger<RedditService> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
        try
        {
            string? accessToken = await GetAccessToken();
            var posts = await GetRedditPosts(accessToken!);
            _logger.LogInformation("Fetched {count} posts from reddit.", posts.Count());
            return posts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured during fetching posts from reddit.");
            return Enumerable.Empty<Post>();
        }
    }

    private async Task<string?> GetAccessToken()
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{RedditClientId}:{RedditClientSecret}"))
        );
        client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

        var content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("username", RedditUsername),
            new KeyValuePair<string, string>("password", RedditPassword)
        ]);

        var response = await client.PostAsync("https://www.reddit.com/api/v1/access_token", content);

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

    private async Task<IEnumerable<Post>> GetRedditPosts(string accessToken, int limit = 50)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

        var response = await client.GetAsync($"https://www.reddit.com/r/funny/top.json?limit={limit}");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        var redditPostResponse = JsonConvert.DeserializeObject<RedditPostResponse>(responseBody)!;

        var posts = _mapper.Map<List<Post>>(redditPostResponse.Data.Children.Select(x => x.Data).ToList());

        return posts;
    }
}
