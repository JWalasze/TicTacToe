using Lib.Models;

namespace Lib.Dtos;

public record GameToBeSavedDto
{
    public int? Player1Id { get; set; }

    public int? Player2Id { get; set; }
}