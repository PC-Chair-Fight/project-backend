using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using project_backend.Models;
using project_backend.Models.Exceptions;
using project_backend.Models.UserController;
using project_backend.Providers.UserProvider;

namespace project_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserProvider _userProvider;

        public UserController(ILogger<UserController> logger, IUserProvider userProvider)
        {
            _logger = logger;
            _userProvider = userProvider;
        }

        [HttpGet]
        [Authorize]
        [Route("User")]
        [ProducesResponseType(typeof(UserResponseObject), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        public IActionResult GetUser(int userId)
        {
            try
            {
                var user = _userProvider.GetUserById(userId);
                return Ok(new UserResponseObject(user));
            }
            catch (ResourceNotFoundException exception)
            {
                return NotFound(new Error(exception.Message));
            }
        }
    }
}
