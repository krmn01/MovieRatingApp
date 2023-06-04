namespace MovieRatingServer.Services.Interfaces
{
    public interface IAuthService
    {
        string GetUserIdFromToken(string token);
    }
}
