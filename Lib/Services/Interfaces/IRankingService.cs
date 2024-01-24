using Lib.Dtos;
using Lib.Enums;
using Lib.Models;

namespace Lib.Services.Interfaces;

public interface IRankingService
{
    public Task<ICollection<PlayerDto>> GetGlobalRanking(int page, int size);

    public Task<ICollection<GameDto>> GetPlayerHistory(int playerId, int page, int size);

    public Task<PlayerDto?> GetPlayerScore(int playerId);

    public Task UpdatePlayerScore(int playerId, GameResult result);
}