using GameFinder_WebAPI.Models;
using GameFinder_WebAPI.Database;
using GameFinder_WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace GameFinder_TestsService
{
    public class GameFinder_TestService
    {
        DbContextOptions<GameDb> GetOptions()
        {
            return new DbContextOptionsBuilder<GameDb>()
                .UseInMemoryDatabase("UnitTest " + DateTime.UtcNow.Millisecond.ToString())
                .Options;
        }

        [Fact]
        public async Task GetItemsByTitle_ReturnNull_WhenTitleIsEmpty()
        {
            await using var content = new GameDb(GetOptions());
            GameService service = new GameService(content);

            Assert.Null(await service.GetGameByTitleAsync(""));
        }

        [Fact]
        public async Task GetItemsByTitle_ReturnNull_WhenObjectDoesNotExist()
        {
            await using var content = new GameDb(GetOptions());
            GameService service = new GameService(content);

            await service.AddGameAsync(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84,
            });

            await service.AddGameAsync(new Game
            {
                name = "The Last of Us",
                production = "Naughty Dog",
                date = "14/06/2013",
                vote = 95

            });

            var result = await service.GetGameByTitleAsync("Final Fantasy 7");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetItemsByTitle_ReturnObject_WhenObjectExist()
        {
            await using var content = new GameDb(GetOptions());
            GameService service = new GameService(content);

            var check = new Game
            {
                name = "The Last of Us",
                production = "Naughty Dog",
                date = "14/06/2013",
                vote = 95
            };

            await service.AddGameAsync(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84,
            });

            await service.AddGameAsync(new Game
            {
                name = "Final Fantasy 7",
                production = "Square Enix",
                date = "31/01/1997",
                vote = 92
            });

            await service.AddGameAsync(check);

            var result = await service.GetGameByTitleAsync("The Last of Us");

            Assert.Equal(check.name, result.name);
            Assert.Equal(check.production, result.production);
            Assert.Equal(check.date, result.date);
            Assert.Equal(check.vote, result.vote);
        }

        [Fact]
        public async Task GetItemsByVote_ReturnEmptyList_WhenParameterIsOutOfRange()
        {
            await using var content = new GameDb(GetOptions());
            GameService service = new GameService(content);

            await service.AddGameAsync(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84
            });

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

            var result = await service.GetGameByVoteAsync(100);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetItemsByVote_ReturnList_WhenParameterIsInRange()
        {
            await using var content = new GameDb(GetOptions());
            GameService service = new GameService(content);

            await service.AddGameAsync(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84,
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
                name = "The Last of Us",
                production = "Naughty Dog",
                date = "14/06/2013",
                vote = 95
            });

            var result = await service.GetGameByVoteAsync(90);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AddItem_ReturnNull_WhenItemAlreadyExists()
        {
            await using var content = new GameDb(GetOptions());
            GameService service = new GameService(content);
            Game game = new Game
            {
                name = "The Last of Us",
                production = "Naughty Dog",
                date = "14/06/2013",
                vote = 95
            };

            await service.AddGameAsync(game);

            await service.AddGameAsync(new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84,
            });

            await service.AddGameAsync(new Game
            {
                name = "Final Fantasy 7",
                production = "Square Enix",
                date = "31/01/1997",
                vote = 92
            });

            (ArgumentException? e, Game? test) = await service.AddGameAsync(game);

            Assert.IsType<ArgumentException>(e);
        }

        [Fact]
        public async Task AddItem_ReturnNull_WhenObjectIsNull()
        {
            await using var content = new GameDb(GetOptions());
            GameService service = new GameService(content);

            (ArgumentException? e, Game? test) = await service.AddGameAsync(NewGame: null);

            Assert.IsType<ArgumentNullException>(e);
        }

        [Fact]
        public async Task UpdateItem_ReturnException_WhenObjectAlreadyExist()
        {
            await using var content = new GameDb(GetOptions());
            GameService service = new GameService(content);
            Game check = new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84,
            };

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

            await service.AddGameAsync(check);

            (ArgumentException? e, Game? test) = await service.UpdateGameAsync("Fallout 4", check);

            Assert.IsType<ArgumentException>(e);
            Assert.Null(test);
        }

        [Fact]
        public async Task UpdateItem_ReturnException_WhenObjectIsNull()
        {
            await using var content = new GameDb(GetOptions());
            GameService service = new GameService(content);
            Game check = new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84,
            };

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

            await service.AddGameAsync(check);

            (ArgumentException? e, Game? test) = await service.UpdateGameAsync("Fallout 4", null);

            Assert.IsType<ArgumentNullException>(e);
            Assert.Null(test);
        }

        [Fact]
        public async Task UpdateItem_ReturnException_WhenObjectDoesNotExist()
        {
            await using var content = new GameDb(GetOptions());
            GameService service = new GameService(content);

            var check = new Game
            {
                name = "Fallout 4",
                production = "Bethesda",
                date = "10/11/2015",
                vote = 84,
            };

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

            await service.AddGameAsync(check);

            (ArgumentException? e, Game? test) = await service.UpdateGameAsync("Fallout 3", check);

            Assert.Null(e);
            Assert.Null(test);
        }

        [Fact]
        public async Task UpdateItem_ReturnItem_WhenObjectExist()
        {
            await using var content = new GameDb(GetOptions());
            GameService service = new GameService(content);

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

            var updated = new Game
            {
                name = "Fallout 3",
                production = "Bethesda",
                date = "28/10/2008",
                vote = 91
            };

            (ArgumentException? e, Game? test) = await service.UpdateGameAsync("Fallout 4", updated);

            Assert.IsType<Game>(test);
            Assert.Null(e);
        }

        [Fact]
        public async Task DeleteItem_ReturnNull_WhenObjectDoesNotExist()
        {
            await using var content = new GameDb(GetOptions());
            GameService service = new GameService(content);

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

            Assert.Null(await service.DeleteGameAsync("Fallout 3"));
        }

        [Fact]
        public async Task DeleteItem_ReturnItem_WhenObjectExist()
        {
            await using var content = new GameDb(GetOptions());
            GameService service = new GameService(content);

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

            Assert.NotNull(await service.DeleteGameAsync("Fallout 4"));
        }
    }
}