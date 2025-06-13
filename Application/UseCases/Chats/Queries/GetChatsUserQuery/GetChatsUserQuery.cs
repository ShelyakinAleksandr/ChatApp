using Application.DbContext;
using Application.UseCases.Chats.ViewModels;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Chats.Queries.GetChatsUserQuery
{
    public class GetChatsUserQuery : IRequest<List<ChatViewModel>>
    {
        public string UserName { get; set; }

        private class Handler : IRequestHandler<GetChatsUserQuery, List<ChatViewModel>>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IChatDbContext _chatDbContext;

            public Handler(ILogger<Handler> logger, IChatDbContext chatDbContext)
            {
                _logger = logger;
                _chatDbContext = chatDbContext;
            }

            public async Task<List<ChatViewModel>> Handle(GetChatsUserQuery query, CancellationToken cancellationToken)
            {
                var user = await _chatDbContext.Users
                   .FirstOrDefaultAsync(u => u.UserName == query.UserName
                                      &&
                                    u.IsSoftDelete == false, cancellationToken);

                if (user is null)
                {
                    _logger.LogError("При получении чатов пользователя не удалось найти пользователя {user}", query.UserName);
                    throw new ArgumentNullException(nameof(user));
                }

                var chats = await _chatDbContext.Chats
                    .Where(c => c.Users.Contains(user))
                    .ToListAsync();
                
                //ToDo: Вынести в DI
                var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Chat, ChatViewModel>();
                });

                var mapper = configuration.CreateMapper();

                var result = mapper.Map<List<ChatViewModel>>(chats);

                return result;
            }
        }
    }
}
