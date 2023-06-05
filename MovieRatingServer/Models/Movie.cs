using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MovieRatingServer.Models
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? ImgSrc { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Director { get; set; }

        public List<Comment>? Comments { get; set; }
        public List<MovieRate>? Rating { get; set; }  
    }
}
