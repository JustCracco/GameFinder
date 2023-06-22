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
        public async Task<IActionResult> AddGame(Game NewGame)
        {
            (var e, var check) = await gameService.AddGameAsync(NewGame);

            if (e is ArgumentNullException)
                return BadRequest();

            if (e is ArgumentException)
                return Conflict("Gioco già presente");

            return StatusCode(StatusCodes.Status201Created, check);
        }

        [HttpPut("title")]
        public async Task<IActionResult> UpdateGame(string titolo, Game UpdateGame)
        {
            (var e, var check) = await gameService.UpdateGameAsync(titolo, UpdateGame);

            if (e is ArgumentNullException)
                return BadRequest();
            else if (e is ArgumentException)
                return Conflict();
            else if (e is null && check == null)
                return BadRequest();

            return StatusCode(StatusCodes.Status202Accepted, check);
        }

        [HttpDelete("title")]
        public async Task<IActionResult> DeleteGame(string titolo)
        {
            var check = await gameService.DeleteGameAsync(titolo);

            if (check == null)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Gioco non trovato");

            return Ok();
        }
    }
}
