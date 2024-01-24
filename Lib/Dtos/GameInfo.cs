using Lib.Enums;

namespace Lib.Dtos;

public class GameInfoBuilder
{
    private readonly GameInfo _gameInfo = new();

    public GameInfoBuilder WithPlayerId(int playerId)
    {
        _gameInfo.PlayerId = playerId;
        return this;
    }

    public GameInfoBuilder WithPlayerUsername(string username)
    {
        _gameInfo.PlayerUsername = username;
        return this;
    }

    public GameInfoBuilder WithOpponentId(int opponentId)
    {
        _gameInfo.OpponentId = opponentId;
        return this;
    }

    public GameInfoBuilder WithOpponentUsername(string opponentUsername)
    {
        _gameInfo.OpponentUsername = opponentUsername;
        return this;
    }

    public GameInfoBuilder WithOpponentConnectionId(string opponentConnectionId)
    {
        _gameInfo.OpponentConnectionId = opponentConnectionId;
        return this;
    }

    public GameInfoBuilder WithGameId(string gameId)
    {
        _gameInfo.GameId = gameId;
        return this;
    }

    public GameInfoBuilder WithPlayerPiece(Piece piece)
    {
        _gameInfo.PlayerPiece = piece;
        return this;
    }

    public GameInfoBuilder WithOpponentPiece(Piece piece)
    {
        _gameInfo.OpponentPiece = piece;
        return this;
    }

    public GameInfo Build()
    {
        return _gameInfo;
    }
}

public record GameInfo
{
    public bool IsPlayerReady { get; set; } = false;

    public int? PlayerId { get; set; }

    public string? PlayerUsername { get; set; }

    public Piece? PlayerPiece { get; set; }

    public bool IsOpponentReady { get; set; } = false;

    public int? OpponentId { get; set; }

    public string? OpponentUsername { get; set; }
    
    public Piece? OpponentPiece { get; set; }

    public string? OpponentConnectionId { get; set; }

    public string? GameId { get; set; }
}