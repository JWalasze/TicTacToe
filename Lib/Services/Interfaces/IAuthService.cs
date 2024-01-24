using Lib.Models;

namespace Lib.Services.Interfaces;

public interface IAuthService
{
    public Task<int> CreateNewPlayer(Player player);

    //TODO: There it will be needed to make better authorization/authentication
    public Task<int> GetIdForUsername(string username);
}