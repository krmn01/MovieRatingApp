using System.ComponentModel.DataAnnotations;

namespace MovieRatingServer.Dtos
{
    public class RegisterRequest
    {
        [Required, EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Compare(nameof(password))]
        public string passwordConfirmation { get; set;} = string.Empty;


        [Required, MaxLength(15), MinLength(3)]
        public string username { get; set; } = string.Empty;
    }
}
