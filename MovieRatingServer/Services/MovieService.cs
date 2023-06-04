using MongoDB.Bson;
using MongoDB.Driver;
using MovieRatingServer.Dtos;
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

        public async Task<bool> AddMovie(Movie movie)
        {
            if (movie == null)
            {
                return false;
            }
            await _mongoService.moviesCollection.InsertOneAsync(movie);
            return true;
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

        public async Task<bool> RateMovie(string userId, RateMovieRequest rate)
        {
            var tmpMovie = await GetMovie(rate.MovieId);

            if (tmpMovie == null) return false;

            var movieRate = new MovieRate
            {
                movieId = rate.MovieId,
                userId = userId,
                rating = rate.Rating
            };

            foreach (var i in tmpMovie.Rating)
            {
                if (i.userId == userId)
                {
                    i.rating = rate.Rating;
                    return await UpdateMovie(tmpMovie);
                }
            }

           

            tmpMovie?.Rating.Add(movieRate);

            return await UpdateMovie(tmpMovie);
        }
    }
}
