using Lib;
using Lib.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using webapi.Hubs;

namespace webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IHubContext<GameHub> _hubContext;
        private readonly ApplicationDbContext _dbDbContext;
        private readonly IGameService _gameService;

        public GameController(IHubContext<GameHub> hubContext, ApplicationDbContext dbDbContext, IGameService gameService)
        {
            _hubContext = hubContext;
            _dbDbContext = dbDbContext;
            _gameService = gameService;
        }


        [HttpPost("FindGame")]
        public async Task<IActionResult> StartLookingForGame([FromBody] string username)
        {
            //Tu trzeba wyjasnic sobie
            var id = HttpContext.Connection.Id;

            Console.WriteLine("WHATEVER " + id);

            var x = _hubContext.Groups.AddToGroupAsync(id, "queue");
            await Task.WhenAll(x);
            if (x.IsCompletedSuccessfully)
            {
                Console.WriteLine("AHA xD");
            }
            await _hubContext.Clients.Group("queue").SendAsync("JoinPreGameQueue");
            //await _gameService.StartLookingForGame(username);
            return Ok();
        }

        //[HttpPost]
        //public async Task<IActionResult> SaveTheGame(...)
        //{
        //    return Ok();
        //}
        //OnDisconnected If something then save the game or not?

        //[HttpPut]
        //public async Task<IActionResult> UpdateEmployee(string companyId)
        //{
        //    return Ok();
        //}

        //[HttpPatch]
        //public async Task<IActionResult> ChangeEmployeeStatus(EmployeeStatus newStatus, string companyId)
        //{
        //    return Ok();
        //}

        //[HttpDelete]
        //public async Task<IActionResult> DeleteEmployee(string companyId)
        //{
        //    return Ok();
        //}
    }
}
