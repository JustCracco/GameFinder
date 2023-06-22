using GameFinder_WebAPI.Models;

namespace GameFinder_WebAPI.Services.IServices
{
    public interface IGameService
    {
        public void StartDb();

        public Task<(ArgumentException?, Game?)> AddGameAsync(Game NewGame);

        public Task<(ArgumentException?, Game?)> UpdateGameAsync(string titolo, Game UpdateGame);

        public Task<Game?> DeleteGameAsync(string titolo);

        public Task<Game?> GetGameByTitleAsync(string titolo);

        public Task<List<Game>> GetGameByVoteAsync(float minvote);
    }
}
