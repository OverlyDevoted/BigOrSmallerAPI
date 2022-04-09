using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly DataContext _context;
        public GameController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Game>>> Get()
        {
            return Ok(await _context.Games.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Game>>> FindByUser(int id)
        {
            var games = await _context.Games.Where(game => game.UserId == id).ToListAsync();
            if (games.Count < 1)
                return BadRequest("No games under that ID");
            return Ok(games);
        }

        [HttpPost]
        public async Task<ActionResult<Game>> Post(GameDto game)
        {
            var user = await _context.Users.FindAsync(game.UserId);
            if (user == null) return BadRequest("No user found");

            Game newGame = new Game {
                Name = game.Name,
                Cover_url = game.Cover_url,
                UserId = game.UserId,
            };
            _context.Games.Add(newGame);
            await _context.SaveChangesAsync();

            return Ok(newGame);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> Delete(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return BadRequest("No such game");
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return Ok(game);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Game>> Update(GameDto game, int id)
        {
            var dbGame = await _context.Games.FindAsync(id);
            if (dbGame == null) return BadRequest("No game found");

            dbGame.Cover_url = game.Cover_url;
            dbGame.Name = game.Name;

            await _context.SaveChangesAsync();
            return Ok(dbGame);
        }

    }
}
