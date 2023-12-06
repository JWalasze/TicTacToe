﻿using Lib.Dtos;
using Lib.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using webapi.Hubs;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IHubContext<GameHub> _hubContext;
    private readonly IGameService _gameService;

    public GameController(IHubContext<GameHub> hubContext, IGameService gameService)
    {
        _hubContext = hubContext;
        _gameService = gameService;
    }


    [HttpPost("SaveGame")]
    public async Task<IActionResult> SaveGame([FromBody] GameToBeSavedDto game)
    {
        await _gameService.SaveGame(game);
        return Ok();
    }
}