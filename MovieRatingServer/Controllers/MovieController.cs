using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRatingServer.Dtos;
using MovieRatingServer.Models;
using MovieRatingServer.Services;
using MovieRatingServer.Services.Interfaces;

namespace MovieRatingServer.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IAuthService _authService;

        public MovieController(IMovieService movieService, IAuthService authService)
        {
            _movieService = movieService;
            _authService = authService;
        }

        private async Task<float> GetRating(string movieId)
        {
            var tmpMovie = await _movieService.GetMovie(movieId);

            float rating = 0.0f;
            foreach(var x in tmpMovie.Rating)
            {
                rating += (float) x.rating;
            }
            return (float)Math.Round((rating / (float)tmpMovie.Rating.Count), 2);
        }



        [HttpGet("{id}")]
        public async Task<GetMovieResponse> GetMovie(string id)
        {
            var tmp = await _movieService.GetMovie(id);
            var rate = await GetRating(id);
            return new GetMovieResponse
            {
                movie = tmp,
                rating = rate,
                ratingCount = tmp.Rating.Count
            };
        }

        [HttpGet]
        [Route("all")]
        public async Task<List<Movie>> GetAllMovies()
        {
            return await _movieService.GetAllMovies();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovie(Movie movie)
        {
            var tmp = await _movieService.UpdateMovie(movie);
            return tmp == true ? Ok() : BadRequest();
        }

        private async Task<bool> RateMovie(string userId, RateMovieRequest rate)
        {
            return await _movieService.RateMovie(userId, rate);
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddMovie([FromBody]Movie movie)
        {
            return await _movieService.AddMovie(movie) ? Ok() : BadRequest();
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteMovie([FromBody]string movieId)
        {
            return await _movieService.DeleteMovie(movieId) ? Ok("Movie deleted") : BadRequest();
        }

        [HttpPost]
        [Route("rate")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> RateMovies([FromBody] RateMovieRequest rate, [FromHeader(Name = "Authorization")] string token)
        {
            var userId = _authService.GetUserIdFromToken(token);
            
            var result = await RateMovie(userId, rate);
            return result ? Ok() : BadRequest();
        }

        private async Task<bool> CommentMovie(string userId, CommentRequest comment)
        {
            return await _movieService.CommentMovie(userId, comment);
        }

        [HttpPost]
        [Route("comment")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CommentMovies([FromBody] CommentRequest comment, [FromHeader(Name = "Authorization")] string token)
        {
            var userId = _authService.GetUserIdFromToken(token);

            var result = await CommentMovie(userId, comment);
            return result ? Ok() : BadRequest();
        }



    }
}
