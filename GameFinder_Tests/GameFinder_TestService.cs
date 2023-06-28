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
                .UseInMemoryDatabase("UnitTestServices " + DateTime.UtcNow.Millisecond.ToString())
                .Options;
        }

        [Fact]
        public async Task GetItemsByTitle_ReturnNull_WhenTitleIsEmpty()
        {
            await using var Content = new GameDb(GetOptions());
            await Content.Database.EnsureDeletedAsync();
            await Content.Database.EnsureCreatedAsync();
            GameService Service = new GameService(Content);
            
            Assert.Null(await Service.GetGameByTitleAsync(""));
        }

        [Fact]
        public async Task GetItemsByTitle_ReturnNull_WhenObjectDoesNotExist()
        {
            await using var Content = new GameDb(GetOptions());
            await Content.Database.EnsureDeletedAsync();
            await Content.Database.EnsureCreatedAsync();
            GameService Service = new GameService(Content);

            var result = await Service.GetGameByTitleAsync("Final Fantasy 8");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetItemsByTitle_ReturnObject_WhenObjectExist()
        {
            await using var Content = new GameDb(GetOptions());
            await Content.Database.EnsureDeletedAsync();
            await Content.Database.EnsureCreatedAsync();
            GameService Service = new GameService(Content);

            var check = new Game
            {
                Name = "The Last of Us",
                Production = "Naughty Dog",
                Date = "14/06/2013",
                Vote = 95
            };

            var result = await Service.GetGameByTitleAsync("The Last of Us");

            Assert.Equal(check.Name, result.Name);
            Assert.Equal(check.Production, result.Production);
            Assert.Equal(check.Date, result.Date);
            Assert.Equal(check.Vote, result.Vote);
        }

        [Fact]
        public async Task GetItemsByVote_ReturnEmptyList_WhenParameterIsOutOfRange()
        {
            await using var Content = new GameDb(GetOptions());
            await Content.Database.EnsureDeletedAsync();
            await Content.Database.EnsureCreatedAsync();
            GameService Service = new GameService(Content);

            var result = await Service.GetGameByVoteAsync(100);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetItemsByVote_ReturnList_WhenParameterIsInRange()
        {
            await using var Content = new GameDb(GetOptions());
            await Content.Database.EnsureDeletedAsync();
            await Content.Database.EnsureCreatedAsync();
            GameService Service = new GameService(Content);

            var result = await Service.GetGameByVoteAsync(90);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AddItem_ReturnException_WhenItemAlreadyExists()
        {
            await using var Content = new GameDb(GetOptions());
            await Content.Database.EnsureDeletedAsync();
            await Content.Database.EnsureCreatedAsync();
            GameService Service = new GameService(Content);
            Game game = new Game
            {
                Name = "The Last of Us",
                Production = "Naughty Dog",
                Date = "14/06/2013",
                Vote = 95
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await Service.AddGameAsync(game));
        }

        [Fact]
        public async Task AddItem_ReturnException_WhenObjectIsNull()
        {
            await using var Content = new GameDb(GetOptions());
            await Content.Database.EnsureDeletedAsync();
            await Content.Database.EnsureCreatedAsync();
            GameService Service = new GameService(Content);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await Service.AddGameAsync(null));
        }

        [Fact]
        public async Task UpdateItem_ReturnException_WhenObjectIsNull()
        {
            await using var Content = new GameDb(GetOptions());
            await Content.Database.EnsureDeletedAsync();
            await Content.Database.EnsureCreatedAsync();
            GameService Service = new GameService(Content);
            Game check = new Game
            {
                Name = "Fallout 4",
                Production = "Bethesda",
                Date = "10/11/2015",
                Vote = 84,
            };

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await Service.UpdateGameAsync("Fallout 4", null));
        }

        [Fact]
        public async Task UpdateItem_ReturnException_WhenObjectDoesNotExist()
        {
            await using var Content = new GameDb(GetOptions());
            await Content.Database.EnsureDeletedAsync();
            await Content.Database.EnsureCreatedAsync();
            GameService Service = new GameService(Content);

            var check = new Game
            {
                Name = "Fallout 4",
                Production = "Bethesda",
                Date = "10/11/2015",
                Vote = 84,
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await Service.UpdateGameAsync("Fallout 3", check));
        }

        [Fact]
        public async Task UpdateItem_ReturnItem_WhenObjectExist()
        {
            await using var Content = new GameDb(GetOptions());
            await Content.Database.EnsureDeletedAsync();
            await Content.Database.EnsureCreatedAsync();
            GameService Service = new GameService(Content);
            var updated = new Game
            {
                Name = "Fallout 3",
                Production = "Bethesda",
                Date = "28/10/2008",
                Vote = 91
            };

            Game test = await Service.UpdateGameAsync("Fallout 4", updated);

            Assert.IsType<Game>(test);
        }

        [Fact]
        public async Task DeleteItem_ReturnNull_WhenObjectDoesNotExist()
        {
            await using var Content = new GameDb(GetOptions());
            await Content.Database.EnsureDeletedAsync();
            await Content.Database.EnsureCreatedAsync();
            GameService Service = new GameService(Content);

            Assert.Null(await Service.DeleteGameAsync("Fallout 3"));
        }

        [Fact]
        public async Task DeleteItem_ReturnItem_WhenObjectExist()
        {
            await using var Content = new GameDb(GetOptions());
            await Content.Database.EnsureDeletedAsync();
            await Content.Database.EnsureCreatedAsync();
            GameService Service = new GameService(Content);

            Assert.NotNull(await Service.DeleteGameAsync("Fallout 4"));
        }
    }
}