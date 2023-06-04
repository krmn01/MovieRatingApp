using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MovieRatingServer.Dtos
{
    public class RegisterResponse
    {
        public string message { get; set; } = string.Empty;
        public bool success { get; set; }  
    }
}
