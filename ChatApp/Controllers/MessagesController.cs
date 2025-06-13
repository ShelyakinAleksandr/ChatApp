using Application.UseCases.Messages.Commands.AddMesgeInChatCommand;
using Application.UseCases.Messages.Queries.GetMessagesFromChatQuery;
using Application.UseCases.Messages.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class MessagesController : BaseControllers
    {
        /// <summary>
        /// Получить историю сообщений чата.
        /// </summary>
        /// <param name="chatId">Id чата.</param>
        /// <returns>Список сообщенией.</returns>
        [HttpGet]
        [Route("{chatId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<MessageViewModel>>> GetMessagesFromChat(Guid chatId)
        { 
            var query = new GetMessagesFromChatQuery { ChatId = chatId };

            var result = await Mediator.Send(query);

            return result;
        }

        [HttpPost]
        [Route("{chatId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> CreateMessage(Guid chatId, [FromForm] string messageText)
        {
            var userName = GetUserName();

            var command = new CreateMesageInChatCommand
            {
                ChatId = chatId,
                UserName = userName,
                MessageText = messageText
            };

            var result = await Mediator.Send(command);

            return result;
        }
    }
}
