using PostAggregator.Api.Data.Entities;
using PostAggregator.Api.Data.Specification;
using PostAggregator.Api.Exceptions;
using System.Data.SQLite;

namespace PostAggregator.Api.Data.Repositories.PostRepository;

public class PostRepository : IPostRepository
{

    private readonly ILogger<PostRepository> _logger;
    private readonly string _connectionString;

    public PostRepository(ILogger<PostRepository> logger, string connectionString)
    {
        _logger = logger;
        _connectionString = connectionString;
    }

    public async Task<PostEntity> CreatePostAsync(PostEntity postEntity)
    {
        using var connection = new SQLiteConnection(_connectionString);
        string query = @"
                    INSERT INTO Posts (Id, Title, Author, CreatedAtUtc, Link, Thumbnail, Source, Text) 
                    VALUES (@Id, @Title, @Author, @CreatedAtUtc, @Link, @Thumbnail, @Source, @Text);";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", postEntity.Id);
        command.Parameters.AddWithValue("@Title", postEntity.Title);
        command.Parameters.AddWithValue("@Author", postEntity.Author);
        command.Parameters.AddWithValue("@CreatedAtUtc", postEntity.CreatedAtUtc);
        command.Parameters.AddWithValue("@Link", postEntity.Link);
        command.Parameters.AddWithValue("@Thumbnail", postEntity.Thumbnail);
        command.Parameters.AddWithValue("@Source", postEntity.Source);
        command.Parameters.AddWithValue("@Text", postEntity.Text ?? string.Empty);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return await GetPostByIdAsync(Guid.Parse(postEntity.Id));
    }

    public async Task<PostEntity> GetPostByIdAsync(Guid id)
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

    public async Task<IEnumerable<PostEntity>> GetPostsAsync(ISpecification specification)
    {
        var query = specification.GetSqlQuery();
        _logger.LogInformation("Executing sql query: {Query}", query);

        using var connection = new SQLiteConnection(_connectionString);
        using var command = new SQLiteCommand(query, connection);

        foreach (var param in specification.GetParameters())
        {
            command.Parameters.AddWithValue(param.Key, param.Value);
        }

        var posts = new List<PostEntity>();

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

    private bool TryReadPost(SQLiteDataReader reader, out PostEntity post)
    {
        try
        {
            post = new PostEntity
            {
                Id = reader["Id"].ToString()!,
                Title = reader["Title"].ToString()!,
                Author = reader["Author"].ToString()!,
                CreatedAtUtc = reader["CreatedAtUtc"].ToString()!,
                Link = reader["Link"].ToString()!,
                Thumbnail = reader["Thumbnail"].ToString()!,
                Source = reader["Source"].ToString()!,
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
