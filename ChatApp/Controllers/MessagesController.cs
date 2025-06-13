using Application.UseCases.Messages.Commands.AddMesgeInChatCommand;
using Application.UseCases.Messages.Queries.GetMessagesFromChatQuery;
using Application.UseCases.Messages.ViewModels;
using ChatApp.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Controllers
{
    public class MessagesController : BaseControllers
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagesController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

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

            //ToDo: Необходимо разделить клиентов которым нужно отправлять сообщение. 
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", command.UserName, command.MessageText);

            return result;
        }
    }
}
