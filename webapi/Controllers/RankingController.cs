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
    public async Task<IActionResult> GetRanking(
        [FromQuery] int page = 1, 
        [FromQuery] int size = 10)
    {
        var result = await _rankingService.GetGlobalRanking(page, size);
        return Ok(result);
    }

    [HttpGet("[action]")]
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
    public async Task<IActionResult> GetPlayerScore([FromQuery] int playerId)
    {
        if (CheckingParameters.AreIntParametersDefault(playerId))
        {
            return BadRequest("Invalid playerId");
        }

        var result = await _rankingService.GetPlayerScore(playerId);

        return result is null ? Problem("Internal Server Error: Player ID not found!", statusCode: 500) : Ok(result);
    }
}