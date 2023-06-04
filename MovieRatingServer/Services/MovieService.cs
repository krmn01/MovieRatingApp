using MongoDB.Bson;
using MongoDB.Driver;
using MovieRatingServer.Models;
using MovieRatingServer.Services.Interfaces;

namespace MovieRatingServer.Services
{
    public class MovieService : IMovieService
    {
        private readonly MongoService _mongoService;

        public MovieService(MongoService mongoService)
        {
            _mongoService = mongoService;
        }
        public async Task<Movie> GetMovie(string id)
        {
            return await _mongoService.moviesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Movie>> GetAllMovies()
        {
            return await _mongoService.moviesCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<bool> UpdateMovie(Movie movie)
        { 
            var tmpMovie = await _mongoService.moviesCollection.ReplaceOneAsync(x => x.Id == movie.Id, movie);
        
            return tmpMovie == null ? false : true;  
        }

        public async Task<bool> RateMovie(Movie movie, User user)
        {
            return false;
        }
    }
}
