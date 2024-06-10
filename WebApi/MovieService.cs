using Microsoft.Extensions.Options;
using MongoDB.Driver;
public class MovieService
{
// Constructor.
// Settings werden per dependency injection übergeben.
public MovieService(IOptions<DatabaseSettings> options)
{
}
public string Check()
{
return "Zugriff auf MongoDB ...";
}
}