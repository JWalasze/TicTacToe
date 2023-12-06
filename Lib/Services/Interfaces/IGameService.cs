using Lib.Dtos;

namespace Lib.Services.Interfaces
{
    public interface IGameService
    {
        public Task SaveGame(GameToBeSavedDto game);
    }
}