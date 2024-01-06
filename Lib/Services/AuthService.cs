using Lib.Dtos;
using Lib.Models;
using Lib.Services.Interfaces;

namespace Lib.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _dbContext;

    public AuthService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateNewPlayer(Player player)
    {
        var result  = await _dbContext.Players.AddAsync(player);
        await _dbContext.SaveChangesAsync();
        return result.Entity.Id;
    }
}