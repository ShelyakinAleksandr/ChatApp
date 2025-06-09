using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : BaseControllers
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Index()
        {
            return NoContent();
        }
    }
}
