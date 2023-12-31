﻿namespace Lib.Models;

public record Player
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; } 

    public int WonGames { get; set; }

    public int LostGames { get; set; }

    public int DrawGames { get; set; }
}