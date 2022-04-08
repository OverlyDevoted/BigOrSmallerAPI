using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            return Ok(await dataContext.Users.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await dataContext.Users.FindAsync(id);
            if (user == null)
                return BadRequest("User not found");
            return Ok(user);
        }

        [HttpPost("find")]
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
        }
        

        [HttpPost]
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
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<User>>> UpdateUser(User user, int id)
        {
            var dbUser = await dataContext.Users.FindAsync(id);
            if(dbUser==null)
                return BadRequest("User not found");
            
            dbUser.Username = user.Username;
            dbUser.Password = user.Password; 
            
            await dataContext.SaveChangesAsync();

            return Ok(await dataContext.Users.ToListAsync());
        }
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
 