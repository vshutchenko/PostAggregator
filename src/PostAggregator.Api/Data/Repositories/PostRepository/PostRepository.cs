using PostAggregator.Api.Data.Entities;
using PostAggregator.Api.Data.Specification;
using PostAggregator.Api.Exceptions;
using PostAggregator.Api.Infrastructure;
using System.Data.SQLite;

namespace PostAggregator.Api.Data.Repositories.PostRepository;

public class PostRepository : IPostRepository
{
    private const string DateFormat = "yyyy-MM-dd HH:mm:ss";
    private readonly ILogger<PostRepository> _logger;
    private readonly string _connectionString;

    public PostRepository(ILogger<PostRepository> logger)
    {
        _logger = logger;
        _connectionString = EnvironmentVariableHelper.GetVariable(EnvironmentVariableHelper.ConnectionString);
    }

    public async Task<Post> CreatePostAsync(Post post)
    {
        using var connection = new SQLiteConnection(_connectionString);
        string query = @"
                    INSERT INTO Post (Id, Title, Author, CreatedAtUtc, Link, Thumbnail, Source, Text) 
                    VALUES (@Id, @Title, @Author, @CreatedAtUtc, @Link, @Thumbnail, @Source, @Text);";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", post.Id.ToString().ToLower());
        command.Parameters.AddWithValue("@Title", post.Title);
        command.Parameters.AddWithValue("@Author", post.Author);
        command.Parameters.AddWithValue("@CreatedAtUtc", post.CreatedAtUtc.ToString(DateFormat));
        command.Parameters.AddWithValue("@Source", post.Source.ToString().ToLower());
        command.Parameters.AddWithValue("@Text", post.Text);
        command.Parameters.AddWithValue("@Link", post.Link);
        command.Parameters.AddWithValue("@Thumbnail", post.Thumbnail);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return await GetPostByIdAsync(post.Id);
    }

    public async Task<Post> GetPostByIdAsync(Guid id)
    {
        var query = "SELECT * FROM Post WHERE Id = @Id";

        using var connection = new SQLiteConnection(_connectionString);
        using var command = new SQLiteCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id.ToString());

        await connection.OpenAsync();
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            if (TryReadPost(reader, out var post))
            {
                return post;
            }
        }

        throw new NotFoundException("Post not found.");
    }

    public async Task<IEnumerable<Post>> GetPostsAsync(ISpecification specification)
    {
        var query = specification.GetSqlQuery();
        _logger.LogInformation("Executing sql query: {Query}", query);

        using var connection = new SQLiteConnection(_connectionString);
        using var command = new SQLiteCommand(query, connection);

        foreach (var param in specification.GetParameters())
        {
            command.Parameters.AddWithValue(param.Key, param.Value);
        }

        var posts = new List<Post>();

        await connection.OpenAsync();
        using var reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            if (TryReadPost(reader, out var post))
            {
                posts.Add(post);
            }
        }

        return posts;
    }

    public async Task<int> GetPostsCountAsync()
    {
        string query = "SELECT COUNT(*) FROM Post;";

        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SQLiteCommand(query, connection);
        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    private bool TryReadPost(SQLiteDataReader reader, out Post post)
    {
        try
        {
            post = new Post
            {
                Id = Guid.Parse(reader["Id"].ToString()!),
                Title = reader["Title"].ToString()!,
                Author = reader["Author"].ToString()!,
                CreatedAtUtc = DateTime.Parse(reader["CreatedAtUtc"].ToString()!),
                Link = reader["Link"].ToString()!,
                Thumbnail = reader["Thumbnail"].ToString()!,
                Source = Enum.Parse<Source>(reader["Source"].ToString()!, true),
                Text = reader["Text"].ToString()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occured during reading post.");
            post = null!;
            return false;
        }

        return true;
    }
}
