using MovieRatingServer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;


namespace MovieRatingServer.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<Movie> _moviesCollection;

        public MongoService(IOptions<MongoSettings> mongoSettings)
        {
            MongoClient client = new MongoClient(mongoSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            _moviesCollection = database.GetCollection<Movie>("movies");
        }
    }
}
