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
            return Ok();
        }
    }
}
