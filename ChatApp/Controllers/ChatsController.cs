using Application.UseCases.Chats.Commands.CreateChatCommand;
using Application.UseCases.Chats.Queries.GetChatsUserQuery;
using Application.UseCases.Chats.ViewModels;
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
        public async Task<ActionResult<List<ChatViewModel>>> GetChatsUser()
        {
            var userName = GetUserName();

            var query = new GetChatsUserQuery { UserName = userName };

            var result = await Mediator.Send(query);

            return result;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> CrateChat([FromForm] CreateChatCommandViewModel addChatCommandViewModel)
        {
            var userName = GetUserName();

            var command = new CreateChatCommand
            { 
                Name = addChatCommandViewModel.Name,
                Description = addChatCommandViewModel.Description,
                UserName = userName,
            };

            var result = await Mediator.Send(command);

            return result;
        }
    }
}
