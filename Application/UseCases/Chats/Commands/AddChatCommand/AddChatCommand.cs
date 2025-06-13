using Application.DbContext;
using Application.Servises;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Chats.Commands.AddChatCommand
{
    public class AddChatCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string UserName { get; set; }

        private class Handler : IRequestHandler<AddChatCommand, Guid>
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

            public async Task<Guid> Handle(AddChatCommand command, CancellationToken cancellationToken)
            {
                var user = await _chatDbContext.Users
                    .FirstOrDefaultAsync(u => u.UserName == command.UserName
                                       &&
                                     u.IsSoftDelete == false, cancellationToken);

                if (user is null)
                {
                    _logger.LogError("При создании чата не удалось найти пользователя {user}", command.UserName);
                    throw new ArgumentNullException(nameof(user));
                }

                var newChat = new Chat
                {
                    Id = Guid.NewGuid(),
                    CreateDate = _dateTimeService.GetCurrentLocalDateTime(),
                    Description = command.Description,
                    Name = command.Name
                };

                newChat.Users.Add(user);
                user.Chats.Add(newChat);

                await _chatDbContext.Chats.AddAsync(newChat, cancellationToken);
                await _chatDbContext.SaveChangesAsync();


                return newChat.Id;
            }
        }
    }
}
