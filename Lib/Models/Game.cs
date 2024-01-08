namespace Lib.Models;

public record Game
{
    public int Id { get; set; }

    public int Player1Id { get; set; }

    public virtual Player Player1 { get; set; }

    public int Player2Id { get; set; }

    public virtual Player Player2 { get; set; }

    public DateTime StarTime { get; set; }

    public DateTime EndTime { get; set; }

    public int? WinnerId { get; set; }

    public virtual Player? Winner { get; set; }
}