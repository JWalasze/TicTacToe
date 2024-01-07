using AutoMapper;
using Lib.Dtos;
using Lib.Models;
using Lib.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateNewPlayer([FromBody] Credentials credentials)
    {
        var newPlayer = _mapper.Map<Player>(credentials);
        var result = await _authService.CreateNewPlayer(newPlayer);
        return Ok(result);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetIdForUsername([FromQuery] string username)
    {
        var result = await _authService.GetIdForUsername(username);
        return Ok(result);
    }
}