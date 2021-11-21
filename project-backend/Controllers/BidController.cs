using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using project_backend.Models.BidController.AddBid;
using project_backend.Models.JobController.AddJob;
using project_backend.Providers.BidProvider;
using System.Linq;
using project_backend.Models.Utils;
using Microsoft.AspNetCore.Http;
using project_backend.Models.BidController.EditBid;
using project_backend.Models.Exceptions;
using project_backend.Models.Bid;
using project_backend.Models;

namespace project_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BidController : ControllerBase
    {
        private readonly ILogger<BidController> _logger;
        private readonly IBidProvider _bidProvider;

        public BidController(ILogger<BidController> logger, IBidProvider bidProvider)
        {
            _logger = logger;
            _bidProvider = bidProvider;
        }

        [Authorize(Roles = "Worker")]
        [HttpPost]
        [ProducesResponseType(typeof(AddBidResponseObject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult AddBid([FromBody] AddBidQueryObject bid)
        {
            var userIdClaim = HttpContext.User.GetUserIdClaim();
            BidDAO newBid;
            try
            {
                newBid = _bidProvider.AddBid(bid.Sum, int.Parse(userIdClaim.Value), bid.JobId);
            }
            catch (NotQualifiedException)
            {
                return Forbid();
            }

            var response = new AddBidResponseObject
            {
                JobId = newBid.JobId,
                WorkerId = newBid.WorkerId,
                Sum = newBid.Sum,
                Id = newBid.Id,
            };
            return Ok(response);
        }

        [Authorize(Roles = "Worker")]
        [HttpPut]
        [ProducesResponseType(typeof(EditBidResponseObject), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        public IActionResult EditBid([FromBody] EditBidQueryObject bid)
        {
            var userIdClaim = HttpContext.User.GetUserIdClaim();
            BidDAO newBid;

            try
            {
                newBid = _bidProvider.EditBid(bid.Sum, int.Parse(userIdClaim.Value), bid.BidId);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new Error(ex.Message));
            }

            var response = new EditBidResponseObject
            {
                JobId = newBid.JobId,
                WorkerId = newBid.WorkerId,
                Sum = newBid.Sum,
                Id = newBid.Id,
            };
            return Ok(response);
        }

    }
}
