using Application.DbContext;
using Application.UseCases.Users.Commands.GetUserCommand;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Users.Commands.GetUserDomainCommand
{
    //ToDo: Удалить класс или переделать.
    public class GetUserCommand :IRequest<UserViewModel>
    {
        public string UserName { get; set; }

        private class Handler : IRequestHandler<GetUserCommand, UserViewModel>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IChatDbContext _chatDbContext;

            public Handler(IChatDbContext chatDbContext, ILogger<Handler> logger)
            {
                _chatDbContext = chatDbContext;
                _logger = logger;
            }

            public async Task<UserViewModel> Handle(GetUserCommand command, CancellationToken cancellationToken)
            {
                var user = await _chatDbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserName == command.UserName
                                            &&
                                          u.IsSoftDelete == false, cancellationToken);


                return new UserViewModel();
            }
        }
    }
}
