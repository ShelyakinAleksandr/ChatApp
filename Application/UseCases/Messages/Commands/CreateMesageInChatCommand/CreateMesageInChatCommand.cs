using Application.DbContext;
using Application.Servises;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Messages.Commands.AddMesgeInChatCommand
{
    public class CreateMesageInChatCommand : IRequest<Guid>
    {
        public Guid ChatId { get; set; }
        public string MessageText { get; set; }

        public string UserName { get; set; }

        private class Handler : IRequestHandler<CreateMesageInChatCommand, Guid>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IChatDbContext _chatDbContext;
            private readonly DateTimeService _dateTimeService;

            public Handler(ILogger<Handler> logger, IChatDbContext chatDbContext, DateTimeService dateTimeService)
            {
                _logger = logger;
                _chatDbContext = chatDbContext;
                _dateTimeService = dateTimeService;
            }

            public async Task<Guid> Handle(CreateMesageInChatCommand command, CancellationToken cancellationToken)
            {
                var user = await _chatDbContext.Users
                       .FirstOrDefaultAsync(u => u.UserName == command.UserName
                                          &&
                                        u.IsSoftDelete == false, cancellationToken);

                if (user is null)
                {
                    _logger.LogError("При создании сообщения не удалось найти пользователя {user}", command.UserName);
                    throw new ArgumentNullException(nameof(user));
                }

                var chat = await _chatDbContext.Chats
                    .Include(c => c.Users)
                    .FirstOrDefaultAsync(c => c.Id == command.ChatId
                                            &&
                                        c.IsSoftDelete == false, cancellationToken);

                if (chat is null)
                {
                    _logger.LogError("При создании сообщения не удалось найти чат с Id: {chatId}", command.ChatId);
                    throw new ArgumentNullException(nameof(user));
                }

                var userInChat = chat.Users.Contains(user);

                if (userInChat is not true)
                {
                    _logger.LogError("При создании сообщения пользователь {user} не состоит в чате {chatId}", user.UserName, chat.Id);
                    throw new ArgumentNullException(nameof(user));
                }

                var message = new Message
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = _dateTimeService.GetCurrentLocalDateTime(),
                    MessageText = command.MessageText,
                    Autor = user,
                    Chat = chat
                };

                await _chatDbContext.Messages.AddAsync(message);
                await _chatDbContext.SaveChangesAsync();

                return message.Id;
            }
        }

        
    }
}
