using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var movieDatabaseConfigSection = builder.Configuration.GetSection("DatabaseSettings"); 
builder.Services.Configure<DatabaseSettings>(movieDatabaseConfigSection);

var app = builder.Build();

app.MapGet("/check", (Microsoft.Extensions.Options.IOptions<DatabaseSettings> options) => {
    try
    {
        // Verbindung zu MongoDB
        var connectionUrl = options.Value.ConnectionString;
        var client = new MongoClient(connectionUrl);

        var databaseNames = client.ListDatabaseNames().ToList();
 
        return "Zugriff auf MongoDB ok. Vorhandene DBs: " + string.Join(",", databaseNames);
    }
    catch (Exception ex)
    {
        // Fehler abfangen und zurückgeben
        return $"Fehler beim Zugriff auf MongoDB: {ex.Message}";
    }
});

app.Run();
