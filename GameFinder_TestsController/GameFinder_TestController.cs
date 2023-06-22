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
        readonly GameDb content;
        readonly GameService service;
        readonly GamesController gamesController;

        DbContextOptions<GameDb> GetOptions()
        {
            return new DbContextOptionsBuilder<GameDb>()
                .UseInMemoryDatabase("UnitTest " + DateTime.UtcNow.Millisecond.ToString())
                .Options;
        }

        public GameFinder_TestController()
        {
            content = new GameDb(GetOptions());
            service = new GameService(content);
            gamesController = new GamesController(service);
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
            await gamesController.AddGame(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84
            });

            await gamesController.AddGame(new Game
            {
                name = "The Last of Us",
                production = "Naughty Dog",
                date = "14/06/2013",
                vote = 95
            });

            await gamesController.AddGame(new Game
            {
                name = "Final Fantasy 7",
                production = "Square Enix",
                date = "31/01/1997",
                vote = 92
            });

            var check = await gamesController.GetGameByTitle("The Last of Us");

            Assert.IsType<OkObjectResult>(check);
        }

        [Fact]
        public async Task GetItemsByVote_ReturnNoContent_WhenParameterIsOutOfRange()
        {
            await gamesController.AddGame(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84
            });

            await gamesController.AddGame(new Game
            {
                name = "The Last of Us",
                production = "Naughty Dog",
                date = "14/06/2013",
                vote = 95
            });

            await gamesController.AddGame(new Game
            {
                name = "Final Fantasy 7",
                production = "Square Enix",
                date = "31/01/1997",
                vote = 92
            });

            var check = await gamesController.GetGameByVote(100);

            Assert.IsType<NoContentResult>(check);
        }

        [Fact]
        public async Task GetItemsByVote_ReturnOk_WhenParameterIsInRange()
        {
            await gamesController.AddGame(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84
            });

            await gamesController.AddGame(new Game
            {
                name = "The Last of Us",
                production = "Naughty Dog",
                date = "14/06/2013",
                vote = 95
            });

            await gamesController.AddGame(new Game
            {
                name = "Final Fantasy 7",
                production = "Square Enix",
                date = "31/01/1997",
                vote = 92
            });

            var check = await gamesController.GetGameByVote(90);

            Assert.IsType<OkObjectResult>(check);
        }

        [Fact]
        public async Task AddItem_ReturnCreated_WhenObjectCreated()
        {
            var check = await gamesController.AddGame(new Game
            {
                name = "Final Fantasy 7",
                production = "Square Enix",
                date = "31/01/1997",
                vote = 92
            });

            Assert.IsType<ObjectResult>(check);
        }

        [Fact]
        public async Task AddItem_ReturnBadRequest_WhenObjectIsNull()
        {
            var check = await gamesController.AddGame(NewGame: null);

            Assert.IsType<BadRequestResult>(check);
        }

        [Fact]
        public async Task AddItem_ReturnConflict_WhenObjectAlreadyExist()
        {
            await gamesController.AddGame(new Game
            {
                name = "Final Fantasy 7",
                production = "Square Enix",
                date = "31/01/1997",
                vote = 92
            });

            Assert.IsType<ConflictObjectResult>(await gamesController.AddGame(new Game
            {
                name = "Final Fantasy 7",
                production = "Square Enix",
                date = "31/01/1997",
                vote = 92
            }));
        }

        [Fact]
        public async Task UpdateItem_ReturnError_WhenObjectDoesNotExist()
        {
            var test = new Game
            {
                name = "Final Fantasy 7",
                production = "Square Enix",
                date = "31/01/1997",
                vote = 92
            };

            await gamesController.AddGame(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84
            });

            await gamesController.AddGame(new Game
            {
                name = "The Last of Us",
                production = "Naughty Dog",
                date = "14/06/2013",
                vote = 95
            });

            await gamesController.AddGame(test);

            var check = await gamesController.UpdateGame("Fallout 3", test);

            Assert.IsType<BadRequestResult>(check);
        }

        [Fact]
        public async Task UpdateItem_ReturnError_WhenObjectAlreadyExist()
        {
            var test = new Game
            {
                name = "Final Fantasy 7",
                production = "Square Enix",
                date = "31/01/1997",
                vote = 92
            };

            await gamesController.AddGame(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84
            });

            await gamesController.AddGame(new Game
            {
                name = "The Last of Us",
                production = "Naughty Dog",
                date = "14/06/2013",
                vote = 95
            });

            await gamesController.AddGame(test);

            var check = await gamesController.UpdateGame("Final Fantasy 7", test);

            Assert.IsType<ConflictResult>(check);
        }

        [Fact]
        public async Task UpdateItem_ReturnError_WhenObjectIsNull()
        {
            await gamesController.AddGame(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84
            });

            await gamesController.AddGame(new Game
            {
                name = "The Last of Us",
                production = "Naughty Dog",
                date = "14/06/2013",
                vote = 95
            });

            var check = await gamesController.UpdateGame("Fallout 3", null);

            Assert.IsType<BadRequestResult>(check);
        }

        [Fact]
        public async Task UpdateItem_ReturnAccepted_WhenObjectExist()
        {

            await gamesController.AddGame(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84
            });

            await gamesController.AddGame(new Game
            {
                name = "The Last of Us",
                production = "Naughty Dog",
                date = "14/06/2013",
                vote = 95
            });

            await gamesController.AddGame(new Game
            {
                name = "Final Fantasy 7",
                production = "Square Enix",
                date = "31/01/1997",
                vote = 92
            });

            var updated = new Game
            {
                name = "Fallout 3",
                production = "Bethesda",
                date = "28/10/2008",
                vote = 91
            };

            var check = await gamesController.UpdateGame("Fallout 4", updated);

            Assert.IsType<ObjectResult>(check);
        }

        [Fact]
        public async Task DeleteItem_ReturnError_WhenObjectDoesNotExist()
        {
            await service.AddGameAsync(new Game
            {
                name = "The Last of Us",
                production = "Naughty Dog",
                date = "14/06/2013",
                vote = 95
            });

            await service.AddGameAsync(new Game
            {
                name = "Final Fantasy 7",
                production = "Square Enix",
                date = "31/01/1997",
                vote = 92
            });

            await service.AddGameAsync(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84,
            });

            var check = await gamesController.DeleteGame("Fallout 3");

            Assert.IsType<ObjectResult>(check);
        }

        [Fact]
        public async Task DeleteItem_ReturnOk_WhenObjectExist()
        {
            await service.AddGameAsync(new Game
            {
                name = "The Last of Us",
                production = "Naughty Dog",
                date = "14/06/2013",
                vote = 95
            });

            await service.AddGameAsync(new Game
            {
                name = "Final Fantasy 7",
                production = "Square Enix",
                date = "31/01/1997",
                vote = 92
            });

            await service.AddGameAsync(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84,
            });

            var check = await gamesController.DeleteGame("Fallout 4");

            Assert.IsType<OkResult>(check);
        }
    }
}