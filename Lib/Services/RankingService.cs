using Lib.Dtos;
using Lib.Enums;
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
            .Where(game => (game.Player1.Id == playerId || 
                            game.Player2.Id == playerId) && 
                           (game.GameStatus == GameStatus.WonByCross || 
                            game.GameStatus == GameStatus.WonByCircle || 
                            game.GameStatus == GameStatus.Draw))
            .Select(game => new GameDto
            {
                Id = game.Id,
                Player1Username = game.Player1.Username,
                Player1Piece = game.Player1Piece,
                Player2Username = game.Player2.Username,
                Player2Piece = game.Player2Piece,
                StartTime = game.StarTime,
                EndTime = game.EndTime,
                WinnerUsername = game.Winner != null ? game.Winner.Username : null,
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

    public async Task UpdatePlayerScore(int playerId, GameResult result)
    {
        var player = await _dbContext.Players.FindAsync(playerId) ?? throw new Exception("Game couldn't be found!");

        if (player is null)
        {
            throw new Exception("Player couldn't be found!");
        }

        switch (result)
        {
            case GameResult.Draw:
                player.DrawGames++;
                break;
            case GameResult.Won:
                player.WonGames++;
                break;
            case GameResult.Loss:
                player.LostGames++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(result), result, "Invalid game result!");
        }

        await _dbContext.SaveChangesAsync();
    }
}