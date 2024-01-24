using Lib.Enums;

namespace Lib.Dtos;

public record GameDto
{
    public string Id { get; set; } = null!;

    public string Player1Username { get; set; } = null!;

    public Piece Player1Piece { get; set; }

    public string Player2Username { get; set; } = null!;

    public Piece Player2Piece { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? WinnerUsername { get; set; }
}