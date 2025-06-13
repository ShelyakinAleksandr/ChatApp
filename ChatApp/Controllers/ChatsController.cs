using System.Security.Claims;
using Application.UseCases.Chats.Commands.AddChatCommand;
using Application.UseCases.Chats.Queries.GetChatsUserQuery;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class ChatsController : BaseControllers
    {
        /// <summary>
        /// Получить список чатов пользователя.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetChatsUser()
        {
            var userName = GetUserName();

            var query = new GetChatsUserQuery { UserName = userName };

            var result = await Mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddChat([FromForm] AddChatCommandViewModel addChatCommandViewModel)
        {
            var userName = GetUserName();

            var command = new AddChatCommand
            { 
                Name = addChatCommandViewModel.Name,
                Description = addChatCommandViewModel.Description,
                UserName = userName,
            };

            var result = await Mediator.Send(command);

            return Ok(result);
        }
    }
}
