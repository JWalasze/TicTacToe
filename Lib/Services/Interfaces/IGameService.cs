using Lib.Dtos;
using Lib.Models;

namespace Lib.Services.Interfaces
{
    public interface IGameService
    {
        public Task<int> SaveGame(Game game);

        public Task UpdateGame(int gameId, int winnerId);
    }
}