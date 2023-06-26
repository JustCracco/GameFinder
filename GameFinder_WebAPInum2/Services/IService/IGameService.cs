using GameFinder_WebAPI.Models;

namespace GameFinder_WebAPI.Services.IServices
{
    public interface IGameService
    {
        public void StartDb();

        public Task<Game> AddGameAsync(Game newGame);

        public Task<Game> UpdateGameAsync(string title, Game updateGame);

        public Task<Game?> DeleteGameAsync(string title);

        public Task<Game?> GetGameByTitleAsync(string title);

        public Task<List<Game>> GetGameByVoteAsync(float minVote);
    }
}
