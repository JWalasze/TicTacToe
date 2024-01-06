using AutoMapper;
using Lib.Dtos;
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
    private readonly IMapper _mapper;

    public GameController(IHubContext<GameHub> hubContext, IGameService gameService, IMapper mapper)
    {
        _hubContext = hubContext;
        _gameService = gameService;
        _mapper = mapper;
    }


    [HttpPost("[action]")]
    public async Task<IActionResult> SaveGame([FromBody] GameToBeSavedDto game)
    {
        await _gameService.SaveGame(game);
        return Ok();
    }
}