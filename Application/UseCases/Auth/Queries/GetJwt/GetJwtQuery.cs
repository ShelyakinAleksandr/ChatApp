﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DbContext;
using Application.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using XSystem.Security.Cryptography;

namespace Application.UseCases.Auth.Queries.GetJwt
{
    public class GetJwtQuery : IRequest<string>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        private class Handler : IRequestHandler<GetJwtQuery, string>
        { 
            private readonly ILogger<Handler> _logger;
            private readonly IChatDbContext _chatDbContext;
            private readonly JwtOptions _jwtOptions;

            public Handler(IChatDbContext chatDbContext, IOptions<JwtOptions> jwtOptions, ILogger<Handler> logger)
            {
                _chatDbContext = chatDbContext;
                _jwtOptions = jwtOptions.Value;
                _logger = logger;
            }

            public async Task<string> Handle(GetJwtQuery command, CancellationToken cancellationToken)
            {
                var user = await _chatDbContext.Users.FirstOrDefaultAsync(u => u.UserName == command.UserName, cancellationToken);

                if (user is null)
                    throw new Exception("Пользователь не найден.");

                var inputBytes = ASCIIEncoding.ASCII.GetBytes(command.Password);
                var passMD5 = new MD5CryptoServiceProvider().ComputeHash(inputBytes);
                var passHash = Convert.ToHexString(passMD5);

                if (user.PasswordHash != passHash)
                    throw new Exception("Неверное имя пользователя или пароль.");

                //ToDo: Добавить id пользователя
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

                var signingCredential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var jwt = new JwtSecurityToken(
                        issuer: _jwtOptions.ValidIssuer,
                        audience: _jwtOptions.ValidAudience,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_jwtOptions.TimeToLive)),
                        signingCredentials: signingCredential
                        );

                _logger.LogInformation("Пользоваетль вошел {user} в систему", user.UserName);

                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
        }
    }
}
