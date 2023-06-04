using Microsoft.AspNetCore.Mvc;
using MovieRatingServer.Dtos;
using MovieRatingServer.Models;

namespace MovieRatingServer.Services.Interfaces
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAllMovies();

        Task<bool> AddMovie(Movie movie); 
        Task<Movie> GetMovie(string id);
        Task<bool> UpdateMovie(Movie movie);

        Task<bool> RateMovie(string userId, RateMovieRequest rate);
       
    }
}
