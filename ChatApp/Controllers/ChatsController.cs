using System.Security.Claims;
using Application.UseCases.Chats.Commands.AddChatCommand;
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetChatsUser()
        {
            ClaimsPrincipal currentUser = User;

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddChat([FromForm] AddChatCommandViewModel addChatCommandViewModel)
        {
            var userName = User.Claims.Where(c => c.Type == ClaimTypes.Name)
                  .Select(c => c.Value).SingleOrDefault();

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
