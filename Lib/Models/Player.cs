namespace Lib.Models;

public record Player
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int WonGames { get; set; } = 0;

    public int LostGames { get; set; } = 0;

    public int DrawGames { get; set; } = 0;
}