using Lib.Dtos;
using Lib.Models;
using Lib.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services;

public class RankingService : IRankingService
{
    private readonly ApplicationDbContext _dbContext;

    public RankingService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<PlayerDto>> GetGlobalRanking(int page, int size)
    {
        var players = await _dbContext.Players
            .Select(player => new PlayerDto
            {
                Id = player.Id,
                Username = player.Username,
                WonGames = player.WonGames,
                LostGames = player.LostGames,
                DrawGames = player.DrawGames
            })
            .OrderByDescending(player => player.WonGames)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
        return players;
    }

    public async Task<ICollection<GameDto>> GetPlayerHistory(int playerId, int page, int size)
    {
        var history = await _dbContext.Games
            .Where(game => game.Player1.Id == playerId || game.Player2.Id == playerId)
            .Select(game => new GameDto
            {
                Id = game.Id,
                Player1Username = game.Player1.Username,
                Player2Username = game.Player2.Username,
                StarTime = game.StarTime,
                EndTime = game.EndTime,
                WinnerUsername = game.Winner!.Username
            })
            .OrderBy(game => game.EndTime)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
        return history;
    }

    public async Task<PlayerDto?> GetPlayerScore(int playerId)
    {
        var score = await _dbContext.Players
            .Where(player => player.Id == playerId)
            .Select(player => new PlayerDto
            {
                Id = player.Id,
                Username = player.Username,
                WonGames = player.WonGames,
                LostGames = player.LostGames,
                DrawGames = player.DrawGames
            })
            .FirstOrDefaultAsync();
        return score;
    }
}