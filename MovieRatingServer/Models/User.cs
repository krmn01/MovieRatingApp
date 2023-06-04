using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;

namespace MovieRatingServer.Models
{
    [CollectionName("Users")]
    public class User :MongoIdentityUser<Guid>
    {
        override public string Email { get; set; } = string.Empty;
        override public string UserName { get; set; } = string.Empty;

        public List<Movie> watchedMovies { get; set; } = null;


    }
}
