using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using project_backend.Models;
using project_backend.Models.Exceptions;
using project_backend.Models.Utils;
using project_backend.Providers.WorkerApplicationProvider;

namespace project_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //Controller that handles interactions regarding a worker and worker applications.
    public class WorkerController : ControllerBase
    {
        private readonly ILogger<WorkerController> _logger;
        private readonly IWorkerApplicationProvider _workerApplicationProvider;

        public WorkerController(ILogger<WorkerController> logger, IWorkerApplicationProvider workerApplicationProvider)
        {
            _logger = logger;
            _workerApplicationProvider = workerApplicationProvider;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("Apply")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        public IActionResult ApplyForWorker()
        {
            var userIdClaim = HttpContext.User.GetUserIdClaim();
            try
            {
                _workerApplicationProvider.AddWorkerApplication(int.Parse(userIdClaim.Value));
            }
            catch (DuplicateWorkerApplicationException ex)
            {
                return Conflict(new Error(ex.Message));
            }
            catch (UserIsAlreadyWorker ex)
            {
                return Forbid(ex.Message);
            }

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "User")]
        [Route("Cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
        public IActionResult CancelWorkApplication()
        {
            var userIdClaim = HttpContext.User.GetUserIdClaim();
            try
            {
                _workerApplicationProvider.CancelWorkerApplication(int.Parse(userIdClaim.Value));
            }
            catch (UserIsAlreadyWorker ex)
            {
                return Conflict(new Error(ex.Message));
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new Error(ex.Message));
            }

            return Ok();
        }

        [HttpPost]
        [Authorize]//This should be locked under the Admin role once we define that entity and role.
        [Route("Accept/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        public IActionResult AcceptWorkerApplication([FromRoute] int userId)
        {
            try
            {
                _workerApplicationProvider.AcceptWorkerApplication(userId);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new Error(ex.Message));
            }
            catch (UserIsAlreadyWorker ex)
            {
                return Conflict(new Error(ex.Message));
            }

            return Ok();
        }

        [HttpDelete]
        [Authorize]//This should be locked under the Admin role once we define that entity and role.
        [Route("Reject/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        public IActionResult RejectWorkerApplication([FromRoute] int userId)
        {
            try
            {
                _workerApplicationProvider.DeleteWorkerApplication(userId);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new Error(ex.Message));
            }

            return Ok();
        }
    }
}
