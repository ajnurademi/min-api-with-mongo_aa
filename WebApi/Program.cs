using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MovieService>();

var movieDatabaseConfigSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(movieDatabaseConfigSection);

var app = builder.Build();

app.MapGet("/", () => "Minimal API nach Arbeitsauftrag 3");

// docker run --name mongodb -d -p 27017:27017 -v data:/data/db -e MONGO_INITDB_ROOT_USERNAME=gbs -e MONGO_INITDB_ROOT_PASSWORD=geheim mongo
app.MapGet("/check", (MovieService movieService) => {
    return movieService.Check();
});

// Insert Movie
app.MapPost("/api/movies", async (Movie movie, MovieService movieService) =>
{
    try
    {
        await movieService.Create(movie);
        return Results.Ok(movie);
    }
    catch (Exception)
    {
        return Results.Conflict();
    }
});

// Get all Movies
app.MapGet("api/movies", async (MovieService movieService) =>
{
    var movies = await movieService.Get();
    return Results.Ok(movies);
});

// Get Movie by id
app.MapGet("api/movies/{id}", async (string id, MovieService movieService) =>
{    
    var movie = await movieService.Get(id);
    if (movie != null)
    {
        return Results.Ok(movie);
    }
    else
    {
        return Results.NotFound();
    }
});

// Update Movie
app.MapPut("/api/movies/{id}", async (string id, Movie movie, MovieService movieService) =>
{
    var updatedMovie = await movieService.Update(id, movie);
    if (updatedMovie != null)
    {
        return Results.Ok(updatedMovie);
    }
    else
    {
        return Results.NotFound();
    }
});

// Delete Movie
app.MapDelete("api/movies/{id}", async (string id, MovieService movieService) =>
{
    var result = await movieService.Remove(id);
    if (result)
    {
        return Results.Ok();
    }
    else
    {
        return Results.NotFound();
    }
});

app.Run();
