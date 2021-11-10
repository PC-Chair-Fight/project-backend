using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using project_backend.Models.JobsController.AddJob;
using project_backend.Models.User;
using project_backend.Providers.UserProvider;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using project_backend.Models.Utils;
using System;

namespace project_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly ILogger<JobsController> _logger;
        private readonly IJobsProvider _jobsProvider;

        public JobsController(ILogger<JobsController> logger, IJobsProvider jobsProvider)
        {
            _logger = logger;
            _jobsProvider = jobsProvider;
        }

        [HttpPost]
        public AddJobResponseObject AddJob([FromBody] AddJobQueryObject job)
        {

            var userIdClaim = HttpContext.User.GetUserIdClaim();

            var newJob = _jobsProvider.AddJob(job.Name, job.Description, int.Parse(userIdClaim.Value));

            var response = new AddJobResponseObject
            {
                AuthorId = newJob.AuthorId,
                DateOfPost = newJob.DateOfPost,
                Description = newJob.Description,
                Done = newJob.Done,
                Id = newJob.Id,
                Name = newJob.Name,

            };
            return response;
        }


    }
}
