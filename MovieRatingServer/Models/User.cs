using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MovieRatingServer.Models
{
    [CollectionName("Users")]
    public class User :MongoIdentityUser<Guid>
    {
        [Required]
        override public string Email { get; set; } = string.Empty;

        [Required, MaxLength(15), MinLength(3)]
        override public string UserName { get; set; } = string.Empty;

        public List<Movie> watchedMovies { get; set; } = null;


    }
}
