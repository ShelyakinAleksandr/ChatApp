using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class BaseControllers : Controller
    {
        private IMediator _mediator;

        protected IMediator Mediator
        {
            get
            {
                return _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
            }
        }

        protected string GetUserName()
        {
            var userName = User.Claims.Where(c => c.Type == ClaimTypes.Name)
                  .Select(c => c.Value).SingleOrDefault();
            return userName;
        }
    }
}
