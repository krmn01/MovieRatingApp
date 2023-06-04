using MovieRatingServer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace MovieRatingServer.Services
{
    public class MongoService
    {
        public IMongoCollection<Movie> moviesCollection;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<Role> roleManager;

        public async Task<User> GetUserById(string Id)
        {
            return await userManager.FindByIdAsync(Id);
        }
        public MongoService(
            IOptions<MongoSettings> mongoSettings,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager)
        {
            MongoClient client = new MongoClient(mongoSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            moviesCollection = database.GetCollection<Movie>("Movies");
            

            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }



    }
}
