using AutoMapper;
using Lib.Dtos;
using Lib.Models;
using Lib.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using webapi.Controllers.Static;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SaveInitialGameState([FromBody] GameInitialStateDto gameInitialState)
    {
        if (CheckingParameters.AreParametersInvalid(
                gameInitialState.GameId, 
                gameInitialState.Player1Id, 
                gameInitialState.Player1Piece, 
                gameInitialState.Player2Id, 
                gameInitialState.Player2Piece))
        {
            return BadRequest("Invalid Initial Game!");
        }
        
        var game = _mapper.Map<Game>(gameInitialState);
        await _gameService.SaveInitialGameState(game);
        return Ok();
    }

    [HttpPatch("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGameForWinner([FromBody] GameUpdateInfoDto gameUpdateInfo)
    {
        if (CheckingParameters.AreParametersInvalid(gameUpdateInfo.GameId))
        {
            return BadRequest("Invalid Game ID!");
        }
        
        try
        {
            await _gameService.UpdateGameForWinner(gameUpdateInfo);
        }
        catch (Exception e)
        {
            return Problem(e.Message, statusCode: 404);
        }

        return Ok();
    }
}