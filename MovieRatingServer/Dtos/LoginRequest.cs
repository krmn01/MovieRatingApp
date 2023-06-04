using System.ComponentModel.DataAnnotations;

namespace MovieRatingServer.Dtos
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string password { get; set; }= string.Empty;

    }

}
