using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static List<User> users = new List<User>
            {
                new User {
                    Id = 1,
                    UserName="K3yman",
                    Passsword="banana16"
                },
                new User
                {
                    Id = 2,
                    UserName="BubbleWrapped",
                    Passsword="dude"
                }
            };
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

        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUser(User user)
        {
            dataContext.Users.Add(user);
            await dataContext.SaveChangesAsync();
            return Ok(await dataContext.Users.ToListAsync());
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<List<User>>> UpdateUser(User user, int id)
        {
            var dbUser = await dataContext.Users.FindAsync(id);
            if(dbUser==null)
                return BadRequest("User not found");
            
            dbUser.UserName = user.UserName;
            dbUser.Passsword = user.Passsword; 
            
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
 