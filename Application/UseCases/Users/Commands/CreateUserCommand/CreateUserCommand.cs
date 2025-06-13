using System.Text;
using Application.DbContext;
using Application.Servises;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using XSystem.Security.Cryptography;

namespace Application.UseCases.Users.Commands.CreateUserCommand
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        private class Handler : IRequestHandler<CreateUserCommand, Guid>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IChatDbContext _chatDbContext;
            private readonly DateTimeService _dateTimeService;

            public Handler(IChatDbContext chatDbContext, ILogger<Handler> logger, DateTimeService dateTimeService)
            {
                _chatDbContext = chatDbContext;
                _logger = logger;
                _dateTimeService = dateTimeService;
            }

            public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                //ToDo: Нужно ли навесить уникальность на поля БД?
                var user = await _chatDbContext.Users
                    .FirstOrDefaultAsync(u => u.UserName == request.UserName, cancellationToken);

                if (user is not null)
                    throw new Exception("Пользователь с таким UserName уже есть в системе");

                var inputBytes = ASCIIEncoding.ASCII.GetBytes(request.Password);
                var passMD5 = new MD5CryptoServiceProvider().ComputeHash(inputBytes);
                var pass = Convert.ToHexString(passMD5);

                var userNew = new User
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = _dateTimeService.GetCurrentLocalDateTime(),
                    Email = request.Email,
                    UserName = request.UserName,
                    PhoneNumber = request.PhoneNumber,
                    FirstName = request.FirstName,
                    PasswordHash = pass
                };

                await _chatDbContext.Users.AddAsync(userNew, cancellationToken);

                await _chatDbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Пользователь {user} зарегестирован", userNew.UserName);

                return userNew.Id;
            }
        }
    }
}
