using GameFinder_WebAPI.Models;
using GameFinder_WebAPI.Database;
using GameFinder_WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using GameFinder_WebAPI.Services.IServices;
using GameFinder_WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GameFinder_TestMoq
{
    public class GameFinder_TestMoq
    {
        private readonly Mock<IGameService> _gameServiceMock;

        public GameFinder_TestMoq()
        {
            _gameServiceMock = new Mock<IGameService>();
        }

        [Fact]
        public async Task GetItemByTitle_ReturnNoContent_WhenObjectDoesNotExist()
        {
            _gameServiceMock.Setup(x => x.GetGameByTitleAsync(""))
                .Returns(Task.FromResult<Game?>(null));
            var TestController = new GamesController(_gameServiceMock.Object);

            Assert.IsType<NoContentResult>(await TestController.GetGameByTitle(""));
        }

        [Fact]
        public async Task GetItemByTitle_ReturnOk_WhenObjectExist()
        {
            Game test = new Game
            {
                Name = "The Last of Us",
                Production = "Naughty Dog",
                Date = "14/06/2013",
                Vote = 95
            };
            _gameServiceMock.Setup(x => x.GetGameByTitleAsync("The Last of Us"))
                .Returns(Task.FromResult<Game?>(test));
            var TestController = new GamesController(_gameServiceMock.Object);

            Assert.IsType<OkObjectResult>(await TestController.GetGameByTitle("The Last of Us"));
        }

        [Fact]
        public async Task GetItemsByVote_ReturnNoContent_WhenParameterIsOutOfRange()
        {
            List<Game> list = new List<Game>();
            _gameServiceMock.Setup(x => x.GetGameByVoteAsync(100))
                .Returns(Task.FromResult(list));
            var TestController = new GamesController(_gameServiceMock.Object);

            Assert.IsType<NoContentResult>(await TestController.GetGameByVote(100));
        }

        [Fact]
        public async Task GetItemsByVote_ReturnOk_WhenParameterIsInRange()
        {
            _gameServiceMock.Setup(x => x.GetGameByVoteAsync(90)).Returns(Task.FromResult(new List<Game>
            {
                new Game
            {
                Id = 1,
                Name = "Final Fantasy 7",
                Production = "Square Enix",
                Date = "31/01/1997",
                Vote = 92
            },

                new Game
            {
                Id = 2,
                Name = "The Last of Us",
                Production = "Naughty Dog",
                Date = "14/06/2013",
                Vote = 95
            }
            }));
            var TestController = new GamesController(_gameServiceMock.Object);

            Assert.IsType<OkObjectResult>(await TestController.GetGameByVote(90));
        }

        [Fact]
        public async Task AddItem_ReturnOk_WhenObjectCreated()
        {
            Game test = new Game
            {
                Name = "Final Fantasy 8",
                Production = "Square Enix",
                Date = "11/02/1999",
                Vote = 90
            };
            _gameServiceMock.Setup(x => x.AddGameAsync(test))
                .Returns(Task.FromResult(test));
            var TestController = new GamesController(_gameServiceMock.Object);

            Assert.IsType<OkObjectResult>(await TestController.AddGame(test));
        }

        [Fact]
        public async Task AddItem_ReturnBadRequest_WhenObjectIsNull()
        {
            _gameServiceMock.Setup(x => x.AddGameAsync(null))
                .Throws(new ArgumentNullException());
            var TestController = new GamesController(_gameServiceMock.Object);

            Assert.IsType<BadRequestResult>(await TestController.AddGame(null));
        }

        [Fact]
        public async Task AddItem_ReturnConflict_WhenObjectAlreadyExist()
        {
            Game test = new Game
            {
                Name = "The Last of Us",
                Production = "Naughty Dog",
                Date = "14/06/2013",
                Vote = 95
            };
            _gameServiceMock.Setup(x => x.AddGameAsync(test))
                .Throws(new ArgumentException());
            GamesController TestController = new GamesController(_gameServiceMock.Object);

            Assert.IsType<ConflictResult>(await TestController.AddGame(test));
        }

        [Fact]
        public async Task UpdateItem_ReturnError_WhenObjectDoesNotExist()
        {
            Game test = new Game
            {
                Name = "The Last of Us",
                Production = "Naughty Dog",
                Date = "14/06/2013",
                Vote = 95
            };
            _gameServiceMock.Setup(x => x.UpdateGameAsync("Cicciobello", test)).Throws(new ArgumentException());
            GamesController TestController = new GamesController(_gameServiceMock.Object);

            var check = await TestController.UpdateGame("Cicciobello", test);

            Assert.IsType<NotFoundResult>(check);
        }

        [Fact]
        public async Task UpdateItem_ReturnError_WhenObjectIsNull()
        {
            _gameServiceMock.Setup(x => x.UpdateGameAsync("Fallout 4", null)).Throws(new ArgumentNullException());
            GamesController TestController = new GamesController(_gameServiceMock.Object);

            Assert.IsType<BadRequestResult>(await TestController.UpdateGame("Fallout 4", null));
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
            _gameServiceMock.Setup(x => x.UpdateGameAsync("Fallout 4", updated))
                .Returns(Task.FromResult(updated));
            GamesController TestController = new GamesController(_gameServiceMock.Object);

            Assert.IsType<OkObjectResult>(await TestController.UpdateGame("Fallout 4", updated));
        }

        [Fact]
        public async Task DeleteItem_ReturnError_WhenObjectDoesNotExist()
        {
            _gameServiceMock.Setup(x => x.DeleteGameAsync("Fallout 3"));
            GamesController TestController = new GamesController(_gameServiceMock.Object);

            Assert.IsType<NotFoundResult>(await TestController.DeleteGame("Fallout 3"));
        }

        [Fact]
        public async Task DeleteItem_ReturnOk_WhenObjectExist()
        {
            Game test = new Game
            {
                Id = 3,
                Name = "Fallout 4",
                Production = "Bethesda",
                Date = "10/11/2015",
                Vote = 84
            };
            _gameServiceMock.Setup(x => x.DeleteGameAsync("Fallout 4"))
                .Returns(Task.FromResult(test));
            GamesController TestController = new GamesController(_gameServiceMock.Object);

            Assert.IsType<OkResult>(await TestController.DeleteGame("Fallout 4"));
        }
    }
}