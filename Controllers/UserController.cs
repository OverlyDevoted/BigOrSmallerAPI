using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext dataContext;

        public UserController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<UserLoginDto>>Get()
        {
            var userId = User?.Identity?.Name;
            var user = await dataContext.Users.FindAsync(int.Parse(userId));
            UserLoginDto loginDto = new UserLoginDto();
            loginDto.UserName = user.Username; 
            return Ok(loginDto);
        }

 /*       [HttpGet("{token}")]
        public async Task<ActionResult<User>> Get(string token)
        {
            var user = await dataContext.Users.FindAsync(id);
            if (user == null)
                return BadRequest("User not found");
            return Ok(user);
        }*/


        //Depricated replaced by ./auth/login
        /*[HttpPost("find")]
        public async Task<ActionResult> FindUser(UserDto user)
        {
            var dbUser = await dataContext.Users
                .Where(users => users.Username == user.Username && users.Password == user.Password)
                .ToListAsync();

            GameUserDto response = new GameUserDto
            {
                Id = dbUser[0].Id,
                Games = await dataContext.Games.Where(games => games.UserId == dbUser[0].Id).ToListAsync()
            };

            return Ok(response);
        }*/


        //Depricated - replaced by ./auth/register
        /*[HttpPost]
        public async Task<ActionResult<List<User>>> AddUser(UserDto user)
        {
            var dbUser = await dataContext.Users
                .Where(users => users.Username == user.Username)
                .ToListAsync();

            if (dbUser.Count > 0)
                return BadRequest("User already exists");
            User newUser = new User{
               Username = user.Username,
               Password = user.Password
            };
            
            dataContext.Users.Add(newUser);
            await dataContext.SaveChangesAsync();

            var forId = await dataContext.Users
                .Where(users => users.Username == user.Username && users.Password == user.Password)
                .ToListAsync();

            return Ok(forId[0].Id);
        }*/

        // TODO Refactor 
        /*[HttpPut("{id}")]
        public async Task<ActionResult<List<User>>> UpdateUser(User user, int id)
        {
            var dbUser = await dataContext.Users.FindAsync(id);
            if(dbUser==null)
                return BadRequest("User not found");
            
            dbUser.Username = user.Username;
            dbUser.Password = user.Password; 
            
            await dataContext.SaveChangesAsync();

            return Ok(await dataContext.Users.ToListAsync());
        }*/
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<User>>> DeleteUser(int id)
        {
            var dbUser = await dataContext.Users.FindAsync(id);
            
            if (dbUser == null)
                return BadRequest("User not found");

            dataContext.Users.Remove(dbUser);
            await dataContext.SaveChangesAsync();
            return Ok(await dataContext.Users.ToListAsync());
        }
    }
}
 