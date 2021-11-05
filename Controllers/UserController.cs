using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using project_backend.Models.User;
using project_backend.Providers;
using project_backend.Repos;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace project_backend.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserProvider _userProvider;

        public UserController(ILogger<UserController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _userProvider = new UserProvider(databaseContext);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public string Login([FromBody] UserDTO user)
        {
            UserDAO userDAO = new UserDAO
            {
                Username = user.Username,
                Password = user.Password
            };

            int userId = _userProvider.getUserByCredentials(userDAO);
            if (userId != -1)
            {
                return GenerateToken(userId);
            }

            return "Error";
        }

        private static string GenerateToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier,userId.ToString()),
                }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(30),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
