using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieRatingServer.Dtos;
using MovieRatingServer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieRatingServer.Controllers
{
    [ApiController]
    [Route("api/authenticate")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;


        public AuthenticationController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("roles/add")]
        public async Task<IActionResult> CreateRole([FromBody]RoleRequest request)
        {
            var newRole = new Role { Name = request.Role };
            var createRole = await _roleManager.CreateAsync(newRole);

            return Ok(new { message = "Role created successfully" });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await RegisterAsync(request);
            return result.success ? Ok(result) : BadRequest(result.message);
        }

        private async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(request.email);
                if(userExists != null) return new RegisterResponse { success = false, message = "Email already used" };

                userExists = new User
                {
                    UserName = request.username,
                    Email = request.email,
                    watchedMovies = new List<Movie>()
                };

                var createUserResult = await _userManager.CreateAsync(userExists, request.password);
                if(createUserResult.Succeeded == false) return new RegisterResponse { success = false, message = "Something went wrong with creating user" };

                var addUserToRole = await _userManager.AddToRoleAsync(userExists, "user");
                if (addUserToRole.Succeeded == false) return new RegisterResponse { success = false, message = "Error when adding role" };

                return new RegisterResponse
                {
                    success = true,
                    message = "Register successful"
                };



            } catch (Exception ex)
            {
                return new RegisterResponse { success = false, message = "Something went wrong" };
            }
        
        }

        private async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.email);
                if (user == null) return new LoginResponse { message = "User does not exist", success = false };

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                var roles = await _userManager.GetRolesAsync(user);

                var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
                claims.AddRange(roleClaims);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("A1f@ds@adghsasda123eafsgqsaffqww2e22da"));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddMinutes(30);

                var token = new JwtSecurityToken
                (
                    issuer: "https://localhost:7108/",
                    audience: "https://localhost:7108/",
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                return new LoginResponse
                {
                    accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    message = "Login successful",
                    email = user?.Email,
                    success = true,
                    userId = user?.Id.ToString()
                };
            }catch(Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return new LoginResponse { success = false, message = "Something went wrong" };
            }

        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await LoginAsync(request);
            return result.success ? Ok(result) : BadRequest(result.message);
        }
    }
}
