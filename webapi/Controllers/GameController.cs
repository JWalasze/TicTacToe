using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IHubContext<> _hubContext;
        private readonly DbContext _dbContext;

        public GameController(IHubContext hubContext, DbContext dbContext)
        {
            _hubContext = hubContext;
            _dbContext = dbContext;
        }


        [HttpPost("FindGame")]
        public async Task StartLookingForGame([FromBody]string username)
        {
            _hubContext.
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAllEmployees(int companyId)
        {
            var result = await _employeeService.GetAllEmployees(companyId);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddNewEmployee(string companyId)
        {
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(string companyId)
        {
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> ChangeEmployeeStatus(EmployeeStatus newStatus, string companyId)
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(string companyId)
        {
            return Ok();
        }
    }
}
