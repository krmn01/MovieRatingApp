using Microsoft.AspNetCore.Authentication;
using MovieRatingServer.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MovieRatingServer.Services
{
    public class AuthService : IAuthService
    {
        string IAuthService.GetUserIdFromToken(string token)
        {
            var tokenWithoutPrefix = token.Replace("Bearer ", "");

            // Decode the token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenWithoutPrefix);

            // Retrieve the user ID claim from the token
            var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }
    }
}
