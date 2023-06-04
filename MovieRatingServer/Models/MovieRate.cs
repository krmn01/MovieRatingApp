using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MovieRatingServer.Models
{
    public class MovieRate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public User? user { get; set; }
        
        public float rating { get; set; }
    }
}
