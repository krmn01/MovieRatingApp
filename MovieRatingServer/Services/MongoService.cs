using MovieRatingServer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;


namespace MovieRatingServer.Services
{
    public class MongoService
    {
        public IMongoCollection<Movie> moviesCollection;

        public MongoService(IOptions<MongoSettings> mongoSettings)
        {
            MongoClient client = new MongoClient(mongoSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            moviesCollection = database.GetCollection<Movie>("Movies");
        }


    }
}
