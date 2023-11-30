namespace Lib.Dtos;

public record PlayerDto
{
    public int Id { get; set; }

    public string Username { get; set; }

    public int WonGames { get; set; }

    public int LostGames { get; set; }

    public int DrawGames { get; set; }
}