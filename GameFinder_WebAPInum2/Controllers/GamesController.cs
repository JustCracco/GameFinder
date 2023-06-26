using Microsoft.AspNetCore.Mvc;
using GameFinder_WebAPI.Models;
using GameFinder_WebAPI.Services.IServices;

namespace GameFinder_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService gameService;

        public GamesController(IGameService context)
        {
            gameService = context;
        }

        [HttpGet("title")]
        public async Task<IActionResult> GetGameByTitle(string title)
        {
            Game? game = await gameService.GetGameByTitleAsync(title);

            if (game == null)
                return NoContent();

            return Ok(game);
        }

        [HttpGet("minimumvote")]
        public async Task<IActionResult> GetGameByVote(float minvote)
        {
            List<Game> game = await gameService.GetGameByVoteAsync(minvote);

            if (game.Count == 0)
                return NoContent();

            return Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> AddGame(Game newGame)
        {
            try
            {
                var check = await gameService.AddGameAsync(newGame);

                return Ok(check);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (ArgumentException)
            {
                return Conflict();
            }
        }

        [HttpPut("title")]
        public async Task<IActionResult> UpdateGame(string title, Game updateGame)
        {
            try
            {
                var check = await gameService.UpdateGameAsync(title, updateGame);

                return Ok(check);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpDelete("title")]
        public async Task<IActionResult> DeleteGame(string title)
        {
            var check = await gameService.DeleteGameAsync(title);

            if (check == null)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Gioco non trovato");

            return Ok();
        }
    }
}
