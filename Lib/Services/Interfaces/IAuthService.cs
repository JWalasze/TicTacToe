using Lib.Models;

namespace Lib.Services.Interfaces;

public interface IAuthService
{
    public Task<int> CreateNewPlayer(Player player);

    public Task<int> GetIdForUsername(string username);
}