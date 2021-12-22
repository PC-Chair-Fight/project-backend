using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using project_backend.Models;
using project_backend.Models.Exceptions;
using project_backend.Models.Exceptions.Universal;
using project_backend.Models.Job;
using project_backend.Models.JobController;
using project_backend.Models.JobController.AddJob;
using project_backend.Models.JobController.EditJob;
using project_backend.Models.JobController.GetJobBids;
using project_backend.Models.JobController.GetJobDetails;
using project_backend.Models.JobController.GetJobs;
using project_backend.Models.Utils;
using project_backend.Providers.BidProvider;
using project_backend.Providers.JobProvider;
using project_backend.Utils;
using System.Collections.Generic;
using System.Linq;

namespace project_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private readonly ILogger<JobController> _logger;
        private readonly IJobProvider _jobProvider;
        private readonly IBidProvider _bidProvider;


        public JobController(ILogger<JobController> logger, IJobProvider jobProvider, IBidProvider bidProvider)
        {
            _logger = logger;
            _jobProvider = jobProvider;
            _bidProvider = bidProvider;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(GetJobsResponseObject), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult GetJobs([FromBody] GetJobsQueryObject query)
        {
            _logger.LogInformation("GetJobs({0})", query);

            if (!int.TryParse(User.FindFirst("Id").Value, out var userId))
            {
                return Unauthorized(new Error("Not logged in"));
            }

            var returnValue = _jobProvider
                .QueryJobs()
                .Include(j => j.Bids)
                    .ThenInclude(b => b.Worker).ThenInclude(w => w.User)
                .If(query.ByCurrentUserOnly ?? false,
                    q => q.Where(j => j.UserId == userId))
                .If(query.OlderThan != null,
                    q => q.Where(j => j.PostDate <= query.OlderThan))
                .If(query.NewerThan != null,
                    q => q.Where(j => j.PostDate >= query.NewerThan))
                .Filter(query.FilterFields, query.FilterValues, query.ExactFilters)
                .Sort(query.OrderBy, query.Ascending)
                .Skip((int)query.Index)
                .Take((int)query.Count)
                .Select(j => new GetJobsResponseObject.GetJobsResponseEntry
                {
                    Id = j.Id,
                    Name = j.Name,
                    Description = j.Description,
                    Done = j.Done,
                    PostDate = j.PostDate,
                    User = new GetJobsResponseObject.FetchedUser
                    {
                        ProfileImage = j.User.ProfileImage,
                        FirstName = j.User.FirstName,
                        LastName = j.User.LastName
                    },
                    Bids = j.Bids.Take(3).Select(bid => new GetJobsResponseObject.FetchedBid
                    {
                        Sum = bid.Sum,
                        Worker = new GetJobsResponseObject.FetchedWorker
                        {
                            User = new GetJobsResponseObject.FetchedUser
                            {
                                ProfileImage = bid.Worker.User.ProfileImage,
                                FirstName = bid.Worker.User.FirstName,
                                LastName = bid.Worker.User.LastName
                            }
                        }
                    }).ToArray()

                }).ToArray();

            _logger.LogInformation("GetJobs -> {0}", returnValue);

            return Ok(new GetJobsResponseObject
            {
                jobs = returnValue
            });
        }

        [HttpPost]
        [Authorize]
        [Route("Add")]
        public JobResponseObject AddJob([FromBody] AddJobQueryObject job)
        {

            var userIdClaim = HttpContext.User.GetUserIdClaim();

            var newJob = _jobProvider.AddJob(job.Name, job.Description, int.Parse(userIdClaim.Value));

            var response = new JobResponseObject
            {
                UserId = newJob.UserId,
                PostDate = newJob.PostDate,
                Description = newJob.Description,
                Done = newJob.Done,
                Id = newJob.Id,
                Name = newJob.Name,
            };
            return response;
        }

        [HttpPut]
        [Authorize]
        [Route("Edit")]
        [ProducesResponseType(typeof(JobResponseObject), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public IActionResult EditJob([FromBody] EditJobQueryObject editedJob)
        {
            var userIdClaim = HttpContext.User.GetUserIdClaim();
            try
            {
                var updatedJob = _jobProvider.EditJob(editedJob.Id, editedJob.Name, editedJob.Description, int.Parse(userIdClaim.Value));
                var response = new JobResponseObject
                {
                    UserId = updatedJob.UserId,
                    PostDate = updatedJob.PostDate,
                    Description = updatedJob.Description,
                    Done = updatedJob.Done,
                    Id = updatedJob.Id,
                    Name = updatedJob.Name,
                };
                return Ok(response);
            }
            catch (ResourceNotFoundException exception)
            {
                return NotFound(new Error(exception.Message));
            }
            catch (UnauthorizedException)
            {
                return Forbid();
            }
        }

        [HttpGet]
        [Authorize]
        [Route("Details")]
        [ProducesResponseType(typeof(JobResponseObject), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        public IActionResult GetJobDetails([FromQuery] GetJobDetailsQueryObject job)
        {
            try
            {
                var requiredJob = _jobProvider.GetJobById(job.Id);
                var response = new JobResponseObject
                {
                    Id = requiredJob.Id,
                    Name = requiredJob.Name,
                    Description = requiredJob.Description,
                    PostDate = requiredJob.PostDate,
                    Done = requiredJob.Done,
                    Images = requiredJob.Images.Select(image => image.URL).ToArray(),
                };
                return Ok(response);
            }
            catch (ResourceNotFoundException exception)
            {
                return NotFound(new Error(exception.Message));
            }

        }

        [HttpGet]
        [Authorize]
        [Route("Bids")]
        [ProducesResponseType(typeof(GetJobBidsResponseObject), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult GetJobBids([FromQuery] GetJobBidsQueryObject query)
        {
            //While we used null-coalescing here, they would never be null because of the validator.
            var index = query.Index ?? 0;
            var jobId = query.JobId ?? 0;
            var count = query.Count ?? 10;

            var bids = _bidProvider.QueryJobBids(jobId)
                .Skip(index)
                .Take(count).ToList();

            return Ok(new GetJobBidsResponseObject(bids));
        }
    }

    static class JobQueryExtensions
    {
        public static IQueryable<JobDAO> Filter(
            this IQueryable<JobDAO> jobs,
            GetJobsQueryObject.FilterField[] filterFields,
            string[] filterValues,
            bool[] exactFilters
        )
        {
            if (filterFields == null || filterValues == null || exactFilters == null)
                return jobs;
            for (int i = 0; i < filterFields.Length; ++i)
            {
                var field = filterFields[i];
                var value = filterValues[i];
                var exact = exactFilters[i];

                jobs = field switch
                {
                    // look for id, ignore exact
                    GetJobsQueryObject.FilterField.Id => jobs.Where(j => j.Id == int.Parse(value)),

                    GetJobsQueryObject.FilterField.Name => exact ?
                            jobs.Where(j => j.Name == value) :
                            jobs.Where(j => j.Name.Contains(value)),

                    GetJobsQueryObject.FilterField.Description => exact ?
                            jobs.Where(j => j.Description == value) :
                            jobs.Where(j => j.Description.Contains(value)),

                    // filter by Done, ignore exact
                    GetJobsQueryObject.FilterField.Done => jobs = jobs.Where(j => j.Done == bool.Parse(value)),

                    _ => jobs
                };
            }
            return jobs;
        }

        public static IOrderedQueryable<JobDAO> BeginSort(
            this IQueryable<JobDAO> jobs,
            GetJobsQueryObject.OrderField orderBy,
            bool ascending
        )
        {
            return orderBy switch
            {
                GetJobsQueryObject.OrderField.Id => ascending ?
                        jobs.OrderBy(j => j.Id) :
                        jobs.OrderByDescending(j => j.Id),

                GetJobsQueryObject.OrderField.Name => ascending ?
                        jobs.OrderBy(j => j.Name) :
                        jobs.OrderByDescending(j => j.Name),

                GetJobsQueryObject.OrderField.Description => ascending ?
                        jobs.OrderBy(j => j.Description) :
                        jobs.OrderByDescending(j => j.Description),

                GetJobsQueryObject.OrderField.PostDate => ascending ?
                        jobs.OrderBy(j => j.PostDate) :
                        jobs.OrderByDescending(j => j.PostDate),

                _ => jobs.OrderBy(j => j.Id)
            };
        }

        public static IQueryable<JobDAO> Sort(
            this IQueryable<JobDAO> jobs,
            GetJobsQueryObject.OrderField[] orderBy,
            bool[] ascending
        )
        {
            if (orderBy == null || orderBy.Length < 1)
            {
                return jobs.OrderBy(j => j.Id);
            }
            var orderChain = jobs.BeginSort(orderBy[0], ascending[0]);
            for (var i = 1; i < orderBy.Length; i++)
            {
                orderChain = orderBy[i] switch
                {
                    GetJobsQueryObject.OrderField.Id => ascending[i] ?
                            orderChain.ThenBy(j => j.Id) :
                            orderChain.ThenByDescending(j => j.Id),

                    GetJobsQueryObject.OrderField.Name => ascending[i] ?
                            orderChain.ThenBy(j => j.Name) :
                            orderChain.ThenByDescending(j => j.Name),

                    GetJobsQueryObject.OrderField.Description => ascending[i] ?
                            orderChain.ThenBy(j => j.Description) :
                            orderChain.ThenByDescending(j => j.Description),

                    GetJobsQueryObject.OrderField.PostDate => ascending[i] ?
                            orderChain.ThenBy(j => j.PostDate) :
                            orderChain.ThenByDescending(j => j.PostDate),

                    _ => jobs.OrderBy(j => j.Id)
                };
            }

            return orderChain;
        }
    }
}
