using Lib.Dtos;
using Lib.Services.Interfaces;

namespace Lib.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _dbContext;

    public AuthService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task CreateNewPlayer(Credentials credentials)
    {
        throw new NotImplementedException();
    }

    public Task<int?> GetIdForUsername(string username)
    {
        throw new NotImplementedException();
    }
}