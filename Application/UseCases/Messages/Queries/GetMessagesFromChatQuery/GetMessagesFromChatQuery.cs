using Application.DbContext;
using Application.UseCases.Messages.ViewModels;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Messages.Queries.GetMessagesFromChatQuery
{
    public class GetMessagesFromChatQuery : IRequest<List<MessageViewModel>>
    {
        public Guid ChatId { get; set; }

        private class Handler : IRequestHandler<GetMessagesFromChatQuery, List<MessageViewModel>>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IChatDbContext _chatDbContext;

            public Handler(ILogger<Handler> logger, IChatDbContext chatDbContext)
            {
                _logger = logger;
                _chatDbContext = chatDbContext;
            }

            public async Task<List<MessageViewModel>> Handle(GetMessagesFromChatQuery query, CancellationToken cancellationToken)
            {
                var messages = await _chatDbContext.Messages
                    .Where(m => m.Chat.Id == query.ChatId
                            &&
                           m.IsSoftDelete == false)
                    .ToListAsync();

                //ToDo: Вынести в DI
                //ToDo: Добавить в маппинг Пользователя
                //ToDo: Возвращать доту в человеческом формате!
                var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Message, MessageViewModel>();
                });

                var mapper = configuration.CreateMapper();

                var result = mapper.Map<List<MessageViewModel>>(messages);

                return result;
            }
        }
    }
}
