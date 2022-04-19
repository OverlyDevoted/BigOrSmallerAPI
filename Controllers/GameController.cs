using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("all/{count}")]
        public async Task<ActionResult<List<Game>>> Get(int count)
        {
            if(count>100 || count<1)
                return BadRequest("Invalid number of requested games");
            var allGames = await _context.Games.ToListAsync();
            if(count < allGames.Count)
            allGames.RemoveRange(0, count);
            allGames = Game.SortByDate(allGames);

            return Ok(allGames);
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<List<Game>>> FindByUser()
        {
            var tokenId = User?.Identity?.Name;

            var games = await _context.Games.Where(game => game.UserId.ToString() == tokenId).ToListAsync();
            if (games.Count < 1)
                return BadRequest("No games under that ID");
            return Ok(games);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<Game>> Post(GameDto game)
        {
            var tokenId = User?.Identity?.Name;
            
            var user = await _context.Users.FindAsync(int.Parse(tokenId));
            if (user == null) return BadRequest("No user found");

            Game newGame = new Game {
                Name = game.Name,
                Cover_url = game.Cover_url,
                UserId = user.Id,
                Created = DateTime.Now,
                IsSmallerMode = game.IsSmallerMode,
            };
            _context.Games.Add(newGame);
            await _context.SaveChangesAsync();

            return Ok(newGame);
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult<Game>> Delete(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return BadRequest("No such game");
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return Ok(game);
        }

        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult<Game>> Update(GameDto game, int id)
        {
            var dbGame = await _context.Games.FindAsync(id);
            if (dbGame == null) return BadRequest("No game found");

            dbGame.Cover_url = game.Cover_url;
            dbGame.Name = game.Name;
            dbGame.IsSmallerMode = game.IsSmallerMode;

            await _context.SaveChangesAsync();
            return Ok(dbGame);
        }

    }
}
