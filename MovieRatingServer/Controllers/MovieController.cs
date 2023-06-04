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



        [HttpGet("{id}")]
        public async Task<Movie> GetMovie(string id)
        {
            return await _movieService.GetMovie(id);
        }

        [HttpGet]
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
        public async Task<IActionResult> AddMovie([FromBody]Movie movie)
        {
            return await _movieService.AddMovie(movie) ? Ok() : BadRequest();
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


        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(string id, )


    }
}
