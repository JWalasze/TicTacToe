namespace Lib.Dtos;

public record GameUpdateInfoDto
{
    public string GameId { get; set; } = null!;

    public int? WinnerId { get; set; }
}