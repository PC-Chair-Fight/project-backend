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
        private readonly DatabaseContext _dbContext;
        private readonly ILogger<UserController> _logger;
        private readonly UserProvider _userProvider;

        public UserController(ILogger<UserController> logger, DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
            _logger = logger;
            _userProvider = new UserProvider(databaseContext);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public string Post()
        {
            string Username = Request.Form["username"];
            string Password = Request.Form["password"];

            UserDAO userDAO = new UserDAO();
            userDAO.Username = Username;
            userDAO.Password = Password;

            int userId = _userProvider.getUserByCredentials(userDAO);
            if (userId != -1)
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
                return token.ToString();
            }

            return "Error";
        }
    }

}
