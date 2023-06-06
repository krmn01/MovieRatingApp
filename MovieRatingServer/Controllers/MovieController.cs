using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRatingServer.Dtos;
using MovieRatingServer.Models;
using MovieRatingServer.Services;
using MovieRatingServer.Services.Interfaces;
using System.Text.RegularExpressions;

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

        private async Task<bool> ValidateMovie(Movie movie)
        {
            if (string.IsNullOrEmpty(movie.Title))
            {
                return false;
            }

            if (string.IsNullOrEmpty(movie.ImgSrc))
            {
                return false;
            }

            if (!movie.ImgSrc.EndsWith(".png"))
            {
                return false;
            }

            if (string.IsNullOrEmpty(movie.Director))
            {
                return false;
            }

            Regex directorRegex = new Regex(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+\s[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+$");
            if (!directorRegex.IsMatch(movie.Director))
            {
                return false;
            }

            if (string.IsNullOrEmpty(movie.Description))
            {
                return false;
            }

            if (movie.ProductionYear < 1920 || movie.ProductionYear > 2023)
            {
                return false;
            }
            return true;
        }

        private async Task<float> GetRating(string movieId)
        {
            var tmpMovie = await _movieService.GetMovie(movieId);

            float rating = 0.0f;
            foreach(var x in tmpMovie.Rating)
            {
                rating += (float) x.rating;
            }
            return tmpMovie.Rating.Count>0 ? (float)Math.Round((rating / (float)tmpMovie.Rating.Count), 2) : 0;
        }



        [HttpGet("{id}")]
        [Route("get/{id}")]
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

        [HttpGet("{id}")]
        [Route(("get/{id}/rate"))]
        [Authorize(Roles = "user")]
        public async Task<GetRateResponse> GetUsersMovieRate([FromHeader(Name = "Authorization")] string token,string id)
        {
            var userId =  _authService.GetUserIdFromToken(token);
            var usersRate = await _movieService.GetMovieRateByUserId(userId, id);
            return new GetRateResponse
            {
                MovieRate = usersRate
            };
        }

        [HttpGet("{query}")]
        [Route("search/{query}")]
        public async Task<List<Movie>> SearchMovies(string query)
        {
            return await _movieService.SearchMovie(query);
        }

        [HttpGet]
        [Route("all")]
         public async Task<List<Movie>> GetAllMovies()
         {
             return await _movieService.GetAllMovies();
         }
      

        [HttpPut]
        [Route("update/this")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateMovie([FromBody]Movie movie)
        {
            var validation = await ValidateMovie(movie);

            if (validation != true) return BadRequest();

            var tmp = await _movieService.UpdateMovie(movie);
            return tmp == true ? Ok() : BadRequest();
        }

        private async Task<bool> RateMovie(string userId, RateMovieRequest rate)
        {
            return await _movieService.RateMovie(userId, rate);
        }

        [HttpPost]
        [Route("set/rate")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> RateMovies([FromBody] RateMovieRequest rate, [FromHeader(Name = "Authorization")] string token)
        {
            var userId = _authService.GetUserIdFromToken(token);

            var result = await RateMovie(userId, rate);
            return result ? Ok() : BadRequest();
        }


        [HttpPost]
        [Route("add/new")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddMovie([FromBody]Movie movie)
        {
            var validation = await ValidateMovie(movie);

            if (validation != true) return BadRequest();

            return await _movieService.AddMovie(movie) ? Ok() : BadRequest();
        }

        [HttpDelete]
        [Route("delete/{movieId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteMovie(string movieId)
        {
            return await _movieService.DeleteMovie(movieId) ? Ok("Movie deleted") : BadRequest();
        }

       
        private async Task<bool> CommentMovie(string userId, CommentRequest comment)
        {
            return await _movieService.CommentMovie(userId, comment);
        }

        [HttpPost]
        [Route("comment/new")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CommentMovies([FromBody] CommentRequest comment, [FromHeader(Name = "Authorization")] string token)
        {
            var userId = _authService.GetUserIdFromToken(token);

            var result = await CommentMovie(userId, comment);
            return result ? Ok() : BadRequest();
        }



    }
}
