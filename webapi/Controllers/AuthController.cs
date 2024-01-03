using Lib.Dtos;
using Lib.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewPlayer(Credentials credentials)
    {
        await _authService.CreateNewPlayer(credentials);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetIdForUsername([FromQuery] string username)
    {
        var result = await _authService.GetIdForUsername(username);
        return Ok(result);
    }
}