using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
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

        private async Task<bool> RateMovie(Movie movie, User user)
        {
            return false;
        }

        [HttpPost]
        [Route("rate/{Id}")]
        [Authorize(Roles = "user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RateMovies(Movie movie, User user)
        {
            var result = await RateMovie(movie, user);
            return result ? Ok() : BadRequest();  
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(string id, )


    }
}
