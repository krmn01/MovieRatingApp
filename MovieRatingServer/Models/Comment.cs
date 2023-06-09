﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MovieRatingServer.Models
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string content { get; set; }

        public string userName { get; set; }

        public string userId {  get; set; }

        public string movieId { get; set; }

        public DateTime dateTime { get; set; }

        public string formatedDataTime { get; set; }
    }
}
