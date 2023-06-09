﻿using Microsoft.AspNetCore.Mvc;
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

        Task<bool> CommentMovie(string userId, CommentRequest comment);

        Task<bool> DeleteMovie(string movieId);

        Task<List<Movie>> SearchMovie(string query);

        Task<MovieRate> GetMovieRateByUserId(string userId, string movieId);
       
    }
}
