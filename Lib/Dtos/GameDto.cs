using Lib.Models;

namespace Lib.Dtos;

public record GameDto
{
    public int Id { get; set; }

    public string Player1Username { get; set; }

    public string Player2Username { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? WinnerUsername { get; set; }
}