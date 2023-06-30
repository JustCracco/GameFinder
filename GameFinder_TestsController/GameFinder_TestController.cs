using GameFinder_WebAPI.Models;
using GameFinder_WebAPI.Database;
using GameFinder_WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using GameFinder_WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace GameFinder_TestsController
{
    public class GameFinder_TestController
    {
        readonly GameDb Content;
        readonly GameService Service;
        readonly GamesController gamesController;

        DbContextOptions<GameDb> GetOptions()
        {
            return new DbContextOptionsBuilder<GameDb>()
                .UseInMemoryDatabase("UnitTestControllers " + DateTime.UtcNow.Millisecond.ToString())
                .Options;
        }

        public GameFinder_TestController()
        {
            Content = new GameDb(GetOptions());
            Content.Database.EnsureDeletedAsync().Wait();
            Content.Database.EnsureCreatedAsync().Wait();
            Service = new GameService(Content);
            gamesController = new GamesController(Service);
        }

        [Fact]
        public async Task GetItemByTitle_ReturnNoContent_WhenObjectDoesNotExist()
        {
            var check = await gamesController.GetGameByTitle("");

            Assert.IsType<NoContentResult>(check);
        }

        [Fact]
        public async Task GetItemByTitle_ReturnOk_WhenObjectExist()
        {
            var check = await gamesController.GetGameByTitle("The Last of Us");

            Assert.IsType<OkObjectResult>(check);
        }

        [Fact]
        public async Task GetItemsByVote_ReturnNoContent_WhenParameterIsOutOfRange()
        {
            var check = await gamesController.GetGameByVote(100);

            Assert.IsType<NoContentResult>(check);
        }

        [Fact]
        public async Task GetItemsByVote_ReturnOk_WhenParameterIsInRange()
        {
            var check = await gamesController.GetGameByVote(90);

            Assert.IsType<OkObjectResult>(check);
        }

        [Fact]
        public async Task AddItem_ReturnOk_WhenObjectCreated()
        {
            Assert.IsType<OkObjectResult>(await gamesController.AddGame(new Game
            {
                Name = "Final Fantasy 8",
                Production = "Square Enix",
                Date = "11/02/1999",
                Vote = 90
            }));
        }

        [Fact]
        public async Task AddItem_ReturnBadRequest_WhenObjectIsNull()
        {
            var check = await gamesController.AddGame(newGame: null);

            Assert.IsType<BadRequestResult>(check);
        }

        [Fact]
        public async Task AddItem_ReturnConflict_WhenObjectAlreadyExist()
        {
            Assert.IsType<ConflictResult>(await gamesController.AddGame(new Game
            {
                Name = "Final Fantasy 7",
                Production = "Square Enix",
                Date = "31/01/1997",
                Vote = 92
            }));
        }

        [Fact]
        public async Task UpdateItem_ReturnError_WhenObjectDoesNotExist()
        {
            var test = new Game
            {
                Name = "Final Fantasy 7",
                Production = "Square Enix",
                Date = "31/01/1997",
                Vote = 92
            };

            var check = await gamesController.UpdateGame("Fallout 3", test);

            Assert.IsType<NotFoundResult>(check);
        }

        [Fact]
        public async Task UpdateItem_ReturnError_WhenObjectIsNull()
        {
            var check = await gamesController.UpdateGame("Fallout 4", null);

            Assert.IsType<BadRequestResult>(check);
        }

        [Fact]
        public async Task UpdateItem_ReturnAccepted_WhenObjectExist()
        {
            var updated = new Game
            {
                Name = "Fallout 3",
                Production = "Bethesda",
                Date = "28/10/2008",
                Vote = 91
            };

            var check = await gamesController.UpdateGame("Fallout 4", updated);

            Assert.IsType<OkObjectResult>(check);
        }

        [Fact]
        public async Task DeleteItem_ReturnError_WhenObjectDoesNotExist()
        {
            var check = await gamesController.DeleteGame("Fallout 3");

            Assert.IsType<NotFoundResult>(check);
        }

        [Fact]
        public async Task DeleteItem_ReturnOk_WhenObjectExist()
        {
            var check = await gamesController.DeleteGame("Fallout 4");

            Assert.IsType<OkResult>(check);
        }
    }
}