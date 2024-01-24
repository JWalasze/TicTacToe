using Lib.Enums;

namespace Lib.Models;

public record Game
{
    public string Id { get; set; } = null!;

    public int Player1Id { get; set; }

    public virtual Player Player1 { get; set; } = null!;

    public Piece Player1Piece { get; set; }

    public int Player2Id { get; set; }

    public virtual Player Player2 { get; set; } = null!;

    public Piece Player2Piece { get; set; }

    public DateTime? StarTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? WinnerId { get; set; }

    public virtual Player? Winner { get; set; }

    public GameStatus GameStatus { get; set; } = GameStatus.BeingPrepared;
}