namespace MovieRatingServer.Dtos
{
    public class LoginResponse
    {
        public bool success { get; set; }
        public string accessToken { get; set; } = string.Empty;
        public string userId { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;

        public string message { get; set; }= string.Empty;
    }
}
