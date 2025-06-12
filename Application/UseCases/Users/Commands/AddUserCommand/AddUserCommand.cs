using System.Text;
using Application.DbContext;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using XSystem.Security.Cryptography;

namespace Application.UseCases.Users.Commands.AddUserCommand
{
    public class AddUserCommand : IRequest<Guid>
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        private class Handler : IRequestHandler<AddUserCommand, Guid>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IChatDbContext _chatDbContext;

            public Handler(IChatDbContext chatDbContext, ILogger<Handler> logger)
            {
                _chatDbContext = chatDbContext;
                _logger = logger;
            }

            public async Task<Guid> Handle(AddUserCommand request, CancellationToken cancellationToken)
            {
                //ToDo: Нужно ли навесить уникальность на поля БД?
                var oldUser = await _chatDbContext.Users
                    .FirstOrDefaultAsync(u => u.UserName == request.UserName);

                if (oldUser is not null)
                    throw new Exception("Пользователь с таким UserName уже есть в системе");

                var inputBytes = ASCIIEncoding.ASCII.GetBytes(request.Password);
                var passMD5 = new MD5CryptoServiceProvider().ComputeHash(inputBytes);
                var pass = Convert.ToHexString(passMD5);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    Email = request.Email,
                    UserName = request.UserName,
                    PhoneNumber = request.PhoneNumber,
                    FirstName = request.FirstName,
                    PasswordHash = pass
                };

                await _chatDbContext.Users.AddAsync(user);

                await _chatDbContext.SaveChangesAsync();

                _logger.LogInformation("Пользователь {user} зарегестирован", user.UserName);

                return user.Id;
            }
        }
    }
}
