using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System.Runtime.Serialization;

namespace MovieRatingServer.Models
{
    [CollectionName("Roles")]
    public class Role :MongoIdentityRole<Guid>
    {
    }
}
