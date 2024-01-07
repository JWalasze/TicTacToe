using Lib.Dtos;
using Lib.Models;
using Lib.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        var result = await _dbContext.Players.AddAsync(player);
        await _dbContext.SaveChangesAsync();
        return result.Entity.Id;
    }

    public async Task<int> GetIdForUsername(string username)
    {
        var result = await _dbContext.Players.FirstAsync(player =>
            player.Username == username
        );
        return result.Id;
    }
}