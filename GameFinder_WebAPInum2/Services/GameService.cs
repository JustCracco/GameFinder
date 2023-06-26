using GameFinder_WebAPI.Database;
using GameFinder_WebAPI.Models;
using GameFinder_WebAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GameFinder_WebAPI.Services
{
    public class GameService : IGameService
    {
        private readonly GameDb Db;

        public GameService(GameDb db)
        {
            this.Db = db;
        }

        public void StartDb()
        {
            //database da implementare ogni volta che si avvia il debug
        }

        public async Task<Game> AddGameAsync(Game newGame)
        {
            if(newGame == null)
                throw new ArgumentNullException();

            var games = await Db.Games.FirstOrDefaultAsync(game => game.Name == newGame.Name);

            if (games != null)
                throw new ArgumentException();

            Db.Games.Add(newGame);

            await Db.SaveChangesAsync();

            return newGame;
        }

        public async Task<Game> UpdateGameAsync(string title, Game updateGame)
        {
            if (updateGame == null)
                throw new ArgumentNullException();

            var game = await Db.Games.FirstOrDefaultAsync(game => game.Name == title);

            if (game == null)
                throw new ArgumentException();

            game.Name = updateGame.Name;
            game.Production = updateGame.Production;
            game.Date = updateGame.Date;
            game.Vote = updateGame.Vote;

            await Db.SaveChangesAsync();

            return game;
        }

        public async Task<Game?> DeleteGameAsync(string title)
        {
            var games = await Db.Games.FirstOrDefaultAsync(game => game.Name == title);

            if(games == null)
                return null;

            Db.Games.Remove(games);
            await Db.SaveChangesAsync();
            return games;
        }

        public async Task<Game?> GetGameByTitleAsync(string title)
        {
            return await Db.Games.FirstOrDefaultAsync(game => game.Name == title);
        }

        public async Task<List<Game>> GetGameByVoteAsync(float minVote)
        {
            List<Game> games = await Db.Games.ToListAsync();

            for (int i = games.Count - 1; i >= 0; i--)
            {
                if (games[i].Vote < minVote)
                    games.RemoveAt(i);
            }

            return games;
        }
    }
}
