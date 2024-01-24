using Lib.Dtos;
using Lib.Models;

namespace Lib.Services.Interfaces
{
    public interface IGameService
    {
        public Task SaveInitialGameState(Game game);

        public Task UpdateGameForWinner(GameUpdateInfoDto gameUpdateInfo);
    }
}