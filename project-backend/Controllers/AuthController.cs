using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using project_backend.Models;
using project_backend.Models.AuthController.Register;
using project_backend.Models.UserController.Login;
using project_backend.Models.Utils;
using project_backend.Providers.UserProvider;
using project_backend.Providers.WorkerProvider;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace project_backend.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserProvider _userProvider;
        private readonly IWorkerProvider _workerProvider;

        public AuthController(ILogger<AuthController> logger, IUserProvider userProvider, IWorkerProvider workerProvider)
        {
            _logger = logger;
            _userProvider = userProvider;
            _workerProvider = workerProvider;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Token), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginQueryObject user)
        {

            var userId = _userProvider.getUserIdByCredentials(user.Email, user.Password);

            if (userId != -1)
            {
                var userIsWorker = _workerProvider.UserIsWorker(userId);
                return Ok(new Token(GenerateToken(userId, userIsWorker ? UserRoles.Worker : UserRoles.User)));
            }
            return Unauthorized(new Error("Wrong credentials"));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Token), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
        public IActionResult Register([FromBody] RegisterQueryObject user)
        {
            var userId = _userProvider.createUser(user.FirstName, user.LastName, user.DateOfBirth, user.Email, user.Password);
            if (userId != -1)
            {
                return Ok(new Token(GenerateToken(userId, UserRoles.User)));
            }
            return Conflict(new Error("Email already exists"));
        }

        private static string GenerateToken(int userId, UserRoles userRole)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", userId.ToString()),
                    new Claim(ClaimTypes.Role, userRole.ToString()),

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
