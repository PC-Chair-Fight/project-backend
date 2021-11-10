using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using project_backend.Models;
using project_backend.Models.Job;
using project_backend.Models.JobController.GetJobs;
using project_backend.Providers.JobProvider;
using System;
using System.Linq;

namespace project_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private readonly ILogger<JobController> _logger;
        private readonly IJobProvider _jobProvider;

        public JobController(ILogger<JobController> logger, IJobProvider jobProvider)
        {
            _logger = logger;
            _jobProvider = jobProvider;
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

            var jobs = _jobProvider.QueryJobs();
            if (query.ByCurrentUserOnly ?? false)
            {
                jobs = jobs.Where(j => j.UserId == userId);
            }

            if (query.OlderThan != null)
            {
                jobs = jobs.Where(j => j.PostDate <= query.OlderThan);
            }

            if (query.NewerThan != null)
            {
                jobs = jobs.Where(j => j.PostDate >= query.NewerThan);
            }

            if (query.FilterFields != null)
            {
                for (int i = 0; i < query.FilterFields.Length; ++i)
                {
                    var field = query.FilterFields[i];
                    var value = query.FilterValues[i];
                    var exact = query.ExactFilters[i];

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
            }

            if (query.OrderBy != null && query.OrderBy.Length >= 1)
            {
                var orderChain = query.OrderBy[0] switch
                {
                    GetJobsQueryObject.OrderField.Id => query.Ascending[0] ?
                            jobs.OrderBy(j => j.Id) :
                            jobs.OrderByDescending(j => j.Id),

                    GetJobsQueryObject.OrderField.Name => query.Ascending[0] ?
                            jobs.OrderBy(j => j.Name) :
                            jobs.OrderByDescending(j => j.Name),

                    GetJobsQueryObject.OrderField.Description => query.Ascending[0] ?
                            jobs.OrderBy(j => j.Description) :
                            jobs.OrderByDescending(j => j.Description),

                    GetJobsQueryObject.OrderField.PostDate => query.Ascending[0] ?
                            jobs.OrderBy(j => j.PostDate) :
                            jobs.OrderByDescending(j => j.PostDate),

                    _ => jobs.OrderBy(j => j.Id)
                };
                for (var i = 0; i < query.OrderBy.Length; i++)
                {
                    orderChain = query.OrderBy[i] switch
                    {
                        GetJobsQueryObject.OrderField.Id => query.Ascending[i] ?
                                orderChain.ThenBy(j => j.Id) :
                                orderChain.ThenByDescending(j => j.Id),

                        GetJobsQueryObject.OrderField.Name => query.Ascending[i] ?
                                orderChain.ThenBy(j => j.Name) :
                                orderChain.ThenByDescending(j => j.Name),

                        GetJobsQueryObject.OrderField.Description => query.Ascending[i] ?
                                orderChain.ThenBy(j => j.Description) :
                                orderChain.ThenByDescending(j => j.Description),

                        GetJobsQueryObject.OrderField.PostDate => query.Ascending[i] ?
                                orderChain.ThenBy(j => j.PostDate) :
                                orderChain.ThenByDescending(j => j.PostDate),
                        
                        _ => jobs.OrderBy(j => j.Id)
                    };
                }

                jobs = orderChain;
            }
            else
            {
                jobs = jobs.OrderBy(j => j.Id);
            }

            jobs = jobs.Skip(query.Index).Take(query.Count);

            var returnValue = jobs.Select(j => new GetJobsResponseObject.GetJobsResponseEntry
            {
                Id = j.Id,
                Name = j.Name,
                Description = j.Description,
                Done = j.Done,
                PostDate = j.PostDate,
                UserId = j.UserId
            }).ToArray();

            _logger.LogInformation("GetJobs -> {0}", returnValue);

            return Ok(new GetJobsResponseObject
            {
                jobs = returnValue
            });
        }
    }
}
