using Lib.Dtos;
using Lib.Enums;
using Lib.Models;
using Lib.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Lib.Services;

public class GameService : IGameService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GameService> _logger;

    public GameService(ApplicationDbContext dbContext, ILogger<GameService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SaveInitialGameState(Game game)
    {
        game.GameStatus = GameStatus.StillInGame;

        await _dbContext.Games.AddAsync(game);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateGameForWinner(GameUpdateInfoDto gameUpdateInfo)
    {
        var game = await _dbContext.Games.FindAsync(gameUpdateInfo.GameId) ?? throw new Exception("Game couldn't be found!");

        if (gameUpdateInfo.WinnerId is not null && game.Player1Id != gameUpdateInfo.WinnerId && game.Player2Id != gameUpdateInfo.WinnerId)
        {
            throw new Exception("Invalid id of the winner!");
        }

        if (gameUpdateInfo.WinnerId is null)
        {
            game.GameStatus = GameStatus.Draw;
            await _dbContext.SaveChangesAsync();
            return;
        }
        
        if (game.Player1Id == gameUpdateInfo.WinnerId)
        {
            game.GameStatus = game.Player1Piece == Piece.Circle ? GameStatus.WonByCircle : GameStatus.WonByCross;
        }
        else if (game.Player2Id == gameUpdateInfo.WinnerId)
        {
            game.GameStatus = game.Player2Piece == Piece.Circle ? GameStatus.WonByCircle : GameStatus.WonByCross;
        }

        game.WinnerId = gameUpdateInfo.WinnerId;
        await _dbContext.SaveChangesAsync();
    }
}