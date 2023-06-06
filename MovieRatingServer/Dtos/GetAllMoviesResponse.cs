using MovieRatingServer.Models;

namespace MovieRatingServer.Dtos
{
    public class GetAllMoviesResponse
    {
        public List<(Movie,float rate, int ratingCount)>? MoviesWithRatings { get; set; }
        
        public GetAllMoviesResponse()
        {
            MoviesWithRatings = new List<(Movie, float rate, int ratingCount)>();
        }
    }
}
