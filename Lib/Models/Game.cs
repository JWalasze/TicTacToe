namespace Lib.Models;

public record Game
{
    public int Id { get; set; }

    public virtual Player Player1 { get; set; }

    public virtual Player Player2 { get; set; }

    public DateTime StarTime { get; set; }

    public DateTime EndTime { get; set; }

    public virtual Player? Winner { get; set; }
}