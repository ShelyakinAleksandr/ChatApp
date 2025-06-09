using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class BaseControllers : Controller
    {
    }
}
