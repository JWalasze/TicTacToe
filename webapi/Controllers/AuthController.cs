using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DbContext _dbContext;

        public AuthController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public Task<IActionResult> CreateNewPlayer([FromBody] Credentials credentials)
        //{

        //}
    }
}
