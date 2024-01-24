using Lib.Enums;
using Lib.Services;
using Lib.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using webapi.Controllers.Static;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RankingController : ControllerBase
{
    private readonly IRankingService _rankingService;

    public RankingController(IRankingService rankingService)
    {
        _rankingService = rankingService;
    }

    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRanking(
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        var result = await _rankingService.GetGlobalRanking(page, size);
        return Ok(result);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPlayerHistory(
        [FromQuery] int playerId,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        if (CheckingParameters.AreIntParametersDefault(playerId))
        {
            return BadRequest("Invalid playerId");
        }

        var result = await _rankingService.GetPlayerHistory(playerId, page, size);
        return Ok(result);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPlayerScore([FromQuery] int playerId)
    {
        if (CheckingParameters.AreIntParametersDefault(playerId))
        {
            return BadRequest("Invalid playerId");
        }

        var result = await _rankingService.GetPlayerScore(playerId);
        return result is null ? Problem("Player ID not found!", statusCode: 400) : Ok(result);
    }

    [HttpPatch("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePlayerScoreForWin([FromBody] int playerId)
    {
        if (CheckingParameters.AreIntParametersDefault(playerId))
        {
            return BadRequest("Invalid playerId");
        }

        try
        {
            await _rankingService.UpdatePlayerScore(playerId, GameResult.Won);
        }
        catch (Exception e)
        {
            return Problem(e.Message, statusCode: 404);
        }

        return Ok();
    }

    [HttpPatch("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePlayerScoreForLoss([FromBody] int playerId)
    {
        if (CheckingParameters.AreIntParametersDefault(playerId))
        {
            return BadRequest("Invalid playerId");
        }

        try
        {
            await _rankingService.UpdatePlayerScore(playerId, GameResult.Won);
        }
        catch (Exception e)
        {
            return Problem(e.Message, statusCode: 404);
        }

        return Ok();
    }

    [HttpPatch("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePlayerScoreForDraw([FromBody] int playerId)
    {
        if (CheckingParameters.AreIntParametersDefault(playerId))
        {
            return BadRequest("Invalid playerId");
        }

        try
        {
            await _rankingService.UpdatePlayerScore(playerId, GameResult.Won);
        }
        catch (Exception e)
        {
            return Problem(e.Message, statusCode: 404);
        }

        return Ok();
    }
}