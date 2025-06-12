using Application.DbContext;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Users.Commands.GetUserDomainCommand
{
    public class GetUserDomainCommand :IRequest<User>
    {
        public string UserName { get; set; }

        private class Handler : IRequestHandler<GetUserDomainCommand, User>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IChatDbContext _chatDbContext;

            public Handler(IChatDbContext chatDbContext, ILogger<Handler> logger)
            {
                _chatDbContext = chatDbContext;
                _logger = logger;
            }

            public async Task<User> Handle(GetUserDomainCommand command, CancellationToken cancellationToken)
            {
                var user = await _chatDbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserName == command.UserName
                                            &&
                                          u.IsSoftDelete == false, cancellationToken);

                if (user is null)
                {
                    _logger.LogError("При создании чата не удалось найти пользователя {user}", command.UserName);
                    throw new ArgumentNullException(nameof(user));
                }

                return user;
            }
        }
    }
}
