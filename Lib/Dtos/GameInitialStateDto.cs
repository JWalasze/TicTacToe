using Lib.Enums;

namespace Lib.Dtos;

public record GameInitialStateDto
{
    public string GameId { get; set; } = null!;

    public int Player1Id { get; set; }

    public Piece Player1Piece { get; set; }

    public int Player2Id { get; set; }

    public Piece Player2Piece { get; set; }
}