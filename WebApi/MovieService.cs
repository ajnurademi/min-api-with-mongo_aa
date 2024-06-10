using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

public class MovieService
{
    private readonly IMongoCollection<Movie> _movies;

    public MovieService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var database = mongoClient.GetDatabase("gbs");
        _movies = database.GetCollection<Movie>("movies");
    }

    public string Check()
    {
        try
        {
            var databaseNames = _movies.Database.Client.ListDatabaseNames().ToList();
            return "Zugriff auf MongoDB ok. Vorhandene DBs: " + string.Join(",", databaseNames);
        }
        catch (System.Exception e)
        {
            return "Zugriff auf MongoDB funktioniert nicht: " + e.Message;
        }
    }

    public async Task Create(Movie movie)
    {
        await _movies.InsertOneAsync(movie);
    }

    public async Task<IEnumerable<Movie>> Get()
    {
        return await _movies.Find(movie => true).ToListAsync();
    }

    public async Task<Movie> Get(string id)
    {
        return await _movies.Find(movie => movie.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Movie> Update(string id, Movie movie)
    {
        var result = await _movies.ReplaceOneAsync(m => m.Id == id, movie);
        return result.IsAcknowledged && result.ModifiedCount > 0 ? movie : null;
    }

    public async Task<bool> Remove(string id)
    {
        var result = await _movies.DeleteOneAsync(movie => movie.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}
