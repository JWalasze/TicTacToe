using Lib.Dtos;
using Lib.Models;
using Lib.Services.Interfaces;

namespace Lib.Services;

public class GameService : IGameService
{
    private readonly ApplicationDbContext _dbContext;

    public GameService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveGame(Game game)
    {
        var result = await _dbContext.Games.AddAsync(game);
        await _dbContext.SaveChangesAsync();
        return result.Entity.Id;
    }

    public async Task UpdateGame(int gameId, int winnerId)
    {
        var game = await _dbContext.Games.FindAsync(gameId);
        if (game == null)
        {
            return;

        }

        game.WinnerId = winnerId;
        await _dbContext.SaveChangesAsync();
    }
}