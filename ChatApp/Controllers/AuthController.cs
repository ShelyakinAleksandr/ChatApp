using Application.UseCases.Auth.Queries.GetJwt;
using Application.UseCases.Users.Commands.CreateUserCommand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseControllers
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Registration([FromForm] CreateUserCommand command)
        { 
            var idUser = await Mediator.Send(command);
            return idUser;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Authentication([FromForm] GetJwtQuery query)
        {
            var jwt = await Mediator.Send(query);

            return jwt;
        }
    }
}
