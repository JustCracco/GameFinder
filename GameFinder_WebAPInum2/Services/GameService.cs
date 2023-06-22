using GameFinder_WebAPI.Database;
using GameFinder_WebAPI.Models;
using GameFinder_WebAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace GameFinder_WebAPI.Services
{
    public class GameService : IGameService
    {
        private readonly GameDb db;

        public GameService(GameDb db)
        {
            this.db = db;
        }

        public void StartDb()
        {
            //database da implementare ogni volta che si avvia il debug
        }

        public async Task<(ArgumentException?, Game?)> AddGameAsync(Game NewGame)
        {
            try
            {
                if(NewGame == null)
                    throw new ArgumentNullException();

                List<Game> games = await db.Games.ToListAsync();

                for (int i = games.Count - 1; i >= 0; i--)
                {
                    if (NewGame.name == games[i].name)
                        throw new ArgumentException();
                }

                db.Games.Add(NewGame);

                await db.SaveChangesAsync();

                return (null, NewGame);
            }
            catch (ArgumentException e)
            {
                return (e, null);
            }
        }

        public async Task<(ArgumentException?, Game?)> UpdateGameAsync(string titolo, Game UpdateGame)
        {
            try
            {
                if (UpdateGame == null)
                    throw new ArgumentNullException();

                List<Game> games = await db.Games.ToListAsync();

                for (int i = 0; i < games.Count; i++)
                {
                    if (games[i].name == titolo)
                    {
                        for(int j = i; j < games.Count; j++)
                        {
                            if (games[j].name == UpdateGame.name)
                                throw new ArgumentException();
                        }

                        if (games[i].name != UpdateGame.name)
                        {
                            games[i].name = UpdateGame.name;
                        }
                        else if (games[i].production != UpdateGame.production)
                        {
                            games[i].production = UpdateGame.production;
                        }
                        else if (games[i].date != UpdateGame.date)
                        {
                            games[i].date = UpdateGame.date;
                        }
                        else if (games[i].vote != UpdateGame.vote)
                        {
                            games[i].vote = UpdateGame.vote;
                        }
                        else
                            break;

                        await db.SaveChangesAsync();

                        return (null, games[i]);
                    }
                }

                return (null, null);
            }
            catch(ArgumentException e)
            {
                return (e, null);
            }
        }

        public async Task<Game?> DeleteGameAsync(string titolo)
        {
            List<Game> games = await db.Games.ToListAsync();

            for (int i = 0; i < games.Count; i++)
            {
                if (games[i].name == titolo)
                {
                    db.Games.Remove(games[i]);
                    await db.SaveChangesAsync();
                    return games[i];
                }
            }

            return null;
        }

        public async Task<Game?> GetGameByTitleAsync(string titolo)
        {
            List<Game> games = await db.Games.ToListAsync();

            for (int i = 0; i < games.Count; i++)
            {
                if (games[i].name == titolo)
                    return games[i];
            }

            return null;
        }

        public async Task<List<Game>> GetGameByVoteAsync(float minvote)
        {
            List<Game> games = await db.Games.ToListAsync();

            for (int i = games.Count - 1; i >= 0; i--)
            {
                if (games[i].vote < minvote)
                    games.RemoveAt(i);
            }

            return games;
        }
    }
}
