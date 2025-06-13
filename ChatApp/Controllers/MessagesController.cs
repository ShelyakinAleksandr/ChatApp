using Application.UseCases.Messages.Commands.AddMesgeInChatCommand;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class MessagesController : BaseControllers
    {
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
