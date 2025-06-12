using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DbContext;
using Application.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using XAct.Messages;
using XAct.Users;
using XSystem.Security.Cryptography;

namespace Application.UseCases.Auth.Queries.GetJwt
{
    public class GetJwtQuery : IRequest<string>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        private class Handler : IRequestHandler<GetJwtQuery, string>
        { 
            private readonly IChatDbContext _chatDbContext;
            private readonly JwtOptions _jwtOptions;

            public Handler(IChatDbContext chatDbContext, IOptions<JwtOptions> jwtOptions)
            {
                _chatDbContext = chatDbContext;
                _jwtOptions = jwtOptions.Value;
            }

            public async Task<string> Handle(GetJwtQuery command, CancellationToken cancellationToken)
            {
                var user = await _chatDbContext.Users.FirstOrDefaultAsync(u => u.UserName == command.UserName);

                if (user is null)
                    throw new Exception("Пользователь не найден.");

                var inputBytes = ASCIIEncoding.ASCII.GetBytes(command.Password);
                var passMD5 = new MD5CryptoServiceProvider().ComputeHash(inputBytes);
                var passHash = Convert.ToHexString(passMD5);

                if (user.PasswordHash != passHash)
                    throw new Exception("Неверный пароль.");

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

                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
        }
    }
}
