using Microsoft.AspNetCore.Mvc;
using MovieRatingServer.Models;

namespace MovieRatingServer.Services.Interfaces
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAllMovies();
        Task<Movie> GetMovie(string id);
        Task<bool> UpdateMovie(Movie movie);

        Task<bool> RateMovie(Movie movie, User user);
       
    }
}
