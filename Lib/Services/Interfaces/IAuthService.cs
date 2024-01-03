using Lib.Dtos;

namespace Lib.Services.Interfaces;

public interface IAuthService
{
    public Task CreateNewPlayer(Credentials credentials);
    public Task<int?> GetIdForUsername(string username);
}