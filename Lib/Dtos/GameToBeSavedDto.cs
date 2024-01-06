using Lib.Models;

namespace Lib.Dtos;

public record GameToBeSavedDto
{
    public virtual Player Player1 { get; set; }

    public virtual Player Player2 { get; set; }

    public virtual Player? Winner { get; set; }
}