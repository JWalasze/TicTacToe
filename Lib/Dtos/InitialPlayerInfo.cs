namespace Lib.Dtos;

public record InitialPlayerInfo
{
    public int? PlayerId { get; set; }

    public bool IsReady { get; set; } = false;
}