namespace Lib.Dtos;

public record Credentials
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;    
}