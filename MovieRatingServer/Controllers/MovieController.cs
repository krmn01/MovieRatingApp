using Microsoft.AspNetCore.Mvc;
using MovieRatingServer.Models;
using MovieRatingServer.Services;

namespace MovieRatingServer.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly MongoService _mongoService;

        public MovieController(MongoService mongoService) 
        {
            _mongoService = mongoService;    
        }
    }
}
