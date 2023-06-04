using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MovieRatingServer.Models
{
    public class MovieRate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? userId { get; set; }

        public string? movieId { get; set; }
        
        public int rating { get; set; }
    }
}
