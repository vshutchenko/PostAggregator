using System.Data.SQLite;

namespace PostAggregator.Api.Data;

public class DbInitializer
{
    private string DbFileName;
    private string ConnectionString;

    public DbInitializer(string dbFileName, string connectionString)
    {
        DbFileName = dbFileName;
        ConnectionString = connectionString;
    }

    public void Initialize()
    {
        if (File.Exists(DbFileName))
        {
            return;
        }

        SQLiteConnection.CreateFile(DbFileName);

        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Post (
                Id TEXT PRIMARY KEY,
                Title TEXT,
                Author TEXT,
                CreatedAtUtc TEXT,
                Link TEXT,
                Thumbnail TEXT,
                Source TEXT,
                Text TEXT);";

        using var command = new SQLiteCommand(createTableQuery, connection);
        command.ExecuteNonQuery();
    }

    public void AddDemoData()
    {
        string insertDemoDataQuery = @"
            INSERT INTO Post (Id, Title, Author, CreatedAtUtc, Link, Thumbnail, Source, Text) 
            VALUES 
            (@id1, @title1, @author1, @createdAtUtc1, @link1, @thumbnail1, @source1, @text1),
            (@id2, @title2, @author2, @createdAtUtc2, @link2, @thumbnail2, @source2, @text2),

            (@id3, @title3, @author3, @createdAtUtc3, @link3, @thumbnail3, @source3, @text3);";

        using var connection = new SQLiteConnection(ConnectionString);
        using var command = new SQLiteCommand(insertDemoDataQuery, connection);

        command.Parameters.AddWithValue("@id1", Guid.NewGuid().ToString());
        command.Parameters.AddWithValue("@title1", "First Post Title");
        command.Parameters.AddWithValue("@author1", "Author1");
        command.Parameters.AddWithValue("@createdAtUtc1", DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@link1", "http://example.com/first-post");
        command.Parameters.AddWithValue("@thumbnail1", "http://example.com/thumbnail1.jpg");
        command.Parameters.AddWithValue("@source1", "PostAggregator");
        command.Parameters.AddWithValue("@text1", "This is the content of the first post.");

        command.Parameters.AddWithValue("@id2", Guid.NewGuid().ToString());
        command.Parameters.AddWithValue("@title2", "Second Post Title");
        command.Parameters.AddWithValue("@author2", "Author2");
        command.Parameters.AddWithValue("@createdAtUtc2", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@link2", "http://example.com/second-post");
        command.Parameters.AddWithValue("@thumbnail2", "http://example.com/thumbnail2.jpg");
        command.Parameters.AddWithValue("@source2", "PostAggregator");
        command.Parameters.AddWithValue("@text2", "This is the content of the second post.");

        command.Parameters.AddWithValue("@id3", Guid.NewGuid().ToString());
        command.Parameters.AddWithValue("@title3", "Third Post Title");
        command.Parameters.AddWithValue("@author3", "Author3");
        command.Parameters.AddWithValue("@createdAtUtc3", DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@link3", "http://example.com/third-post");
        command.Parameters.AddWithValue("@thumbnail3", "http://example.com/thumbnail3.jpg");
        command.Parameters.AddWithValue("@source3", "PostAggregator");
        command.Parameters.AddWithValue("@text3", "This is the content of the third post.");

        connection.Open();
        command.ExecuteNonQuery();
    }
}
