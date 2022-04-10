using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameItemController : ControllerBase
    {
        private readonly DataContext _context;
        public GameItemController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Game>>> Get()
        {
            return Ok(await _context.GameItems.ToListAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<GameItem>>> FindByGame(int id)
        {
            var games = await _context.GameItems.Where(game => game.GameId == id).ToListAsync();
            if (games.Count < 1)
                return BadRequest("No games under that ID");
            return Ok(games);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<GameItem>> AddGameItem(GameItemDto gameItem, int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return BadRequest("No such game found");
            GameItem newGameItem = new GameItem
            {
                GameId = id,
                Name = gameItem.Name,
                Cover_Url = gameItem.Cover_Url,
                Score = gameItem.Score,
            };

            _context.GameItems.Add(newGameItem);
            await _context.SaveChangesAsync();

            return Ok(newGameItem);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameItem>> Delete(int id)
        {
            var game = await _context.GameItems.FindAsync(id);
            if (game == null) return BadRequest("No such game");
            _context.GameItems.Remove(game);
            await _context.SaveChangesAsync();
            return Ok(game);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<GameItem>> Update(GameItemDto game, int id)
        {
            var dbGame = await _context.GameItems.FindAsync(id);
            if (dbGame == null) return BadRequest("No game found");

            dbGame.Cover_Url = game.Cover_Url;
            dbGame.Name = game.Name;
            dbGame.Score = game.Score;

            await _context.SaveChangesAsync();
            return Ok(dbGame);
        }
    }
}
