using MovieRatingServer.Models;

namespace MovieRatingServer.Dtos
{
    public class GetMovieResponse
    {
        public Movie movie { get; set; }
        public float rating {  get; set; }

        public int ratingCount { get; set; }
    }
}
