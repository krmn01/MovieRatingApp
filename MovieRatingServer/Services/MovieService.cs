using MongoDB.Bson;
using MongoDB.Driver;
using MovieRatingServer.Dtos;
using MovieRatingServer.Models;
using MovieRatingServer.Services.Interfaces;
using System.Net.WebSockets;

namespace MovieRatingServer.Services
{
    public class MovieService : IMovieService
    {
        private readonly MongoService _mongoService;

        public MovieService(MongoService mongoService)
        {
            _mongoService = mongoService;
        }

        public async Task<bool> DeleteMovie(string movieId)
        {
            var tmpMovie = await _mongoService.moviesCollection.Find(x => x.Id == movieId).FirstOrDefaultAsync();
            if (tmpMovie == null) return false;
            _mongoService.moviesCollection.DeleteOne(a => a.Id == movieId);

            return true;
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

            if(rate.Rating < 1 || rate.Rating>5) rate.Rating = 3;


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

        public async Task<bool> CommentMovie(string userId, CommentRequest comment)
        {
            var tmpMovie = await GetMovie(comment.MovieId);
            var tmpUser = await _mongoService.GetUserById(userId);

            if (tmpMovie == null || tmpUser == null|| comment.Content == string.Empty) return false;

            var tmpDateTime = DateTime.Now;
            
            var newComment = new Comment
            {
                content = comment.Content,
                movieId = comment.MovieId,
                userId = userId,
                userName = tmpUser.UserName,
                dateTime = tmpDateTime,
                formatedDataTime = tmpDateTime.ToString("HH:mm dd/MM/yyyy")
        };

            tmpMovie?.Comments.Add(newComment);

            return await UpdateMovie(tmpMovie);
        }

        public async Task<List<Movie>> SearchMovie(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return await _mongoService.moviesCollection.Find(new BsonDocument()).ToListAsync();
            }

            var filter = Builders<Movie>.Filter.Regex("Title", new BsonRegularExpression(query, "i"));

            var result = await _mongoService.moviesCollection.Find(filter).ToListAsync();

            return result;
        }
    }
}
