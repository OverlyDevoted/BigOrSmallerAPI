using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly DataContext dataContext;

        public AuthController(IConfiguration configuration, DataContext dataContext)
        {
            this.configuration = configuration;
            this.dataContext = dataContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var users = await dataContext.Users.Where(found => found.Username == request.Username).ToListAsync();
            if (users.Count > 0)
                return BadRequest("Such user already exists");

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User();
            user.Username = request.Username;
            user.PasswordHash = passwordHash; 
            user.PasswordSalt = passwordSalt;

            dataContext.Users.Add(user);
            await dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var foundUser = await dataContext.Users.Where(user => user.Username == request.Username).ToListAsync();
            if (foundUser.Count < 1)
                return BadRequest("User not found");

            if (!VerifyPasswordHash(request.Password, foundUser[0].PasswordHash, foundUser[0].PasswordSalt))
                return BadRequest("Wrong password");

            string token = CreateToken(foundUser[0]);
            return Ok(token);
        }

        

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        private bool VerifyPasswordHash(string password, byte[]passwordHash, byte[]passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>() { new Claim(ClaimTypes.Name, user.Id.ToString()) };

            //json web token
            var key = new SymmetricSecurityKey(System.Text.Encoding
                .UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
