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

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(string id, )


    }
}
