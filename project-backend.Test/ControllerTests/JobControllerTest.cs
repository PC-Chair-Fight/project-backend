using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using project_backend.Controllers;
using project_backend.Models.Bid;
using project_backend.Models.Job;
using project_backend.Models.JobController;
using project_backend.Models.JobController.GetJobDetails;
using project_backend.Models.JobController.GetJobs;
using project_backend.Models.JobImage;
using project_backend.Models.User;
using project_backend.Providers.BidProvider;
using project_backend.Providers.JobProvider;
using System;
using System.Linq;
using System.Security.Claims;
using Xunit;
using Xunit.Abstractions;

namespace project_backend.Test.ControllerTests
{
    [Trait("Category", "Unit")]
    public class JobControllerTest
    {
        private readonly JobController _controller;
        private Mock<IJobProvider> _jobProvider;
        private Mock<IBidProvider> _bidProvider;

        public JobControllerTest(ITestOutputHelper output)
        {
            var users = new UserDAO[]
            {
                new UserDAO
                {
                    Id = 1,
                    Email = "user1@gmail.com",
                    DateOfBirth = new DateTime(2020, 2, 20),
                    FirstName = "User1",
                    LastName = "User1",
                    Password = ""
                },
                new UserDAO
                {
                    Id = 2,
                    Email = "user2@gmail.com",
                    DateOfBirth = new DateTime(2021, 1, 1),
                    FirstName = "User2",
                    LastName = "User2",
                    Password = ""
                }
            };

            _jobProvider = new Mock<IJobProvider>();

            _bidProvider = new Mock<IBidProvider>();

            _jobProvider.Setup(x => x.QueryJobs()).Returns(Enumerable.Range(0, 5)
                .Select(i =>
                    new JobDAO
                    {
                        Id = i,
                        Name = $"Job{i + 1}",
                        Description = $"Job{i + 1}Desc",
                        User = users[i % 2],
                        UserId = users[i % 2].Id,
                        PostDate = i % 2 == 0 ? new DateTime(2020, 3, 20 + i / 2) : new DateTime(2021, 2, 1 + i / 2),
                        Done = i % 2 != 0,
                        Images = new JobImageDAO[0],
                        Bids = new BidDAO[0]
                    }
                )
                .Append(new JobDAO
                {
                    Name = "Job5",
                    Description = "Job5Desc",
                    User = users[0],
                    UserId = users[0].Id,
                    PostDate = new DateTime(2020, 3, 21),
                    Done = false,
                    Images = new JobImageDAO[0],
                    Bids = new BidDAO[0]
                })
                .AsQueryable());

            _controller = new JobController(NullLogger<JobController>.Instance, _jobProvider.Object, _bidProvider.Object);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("Id", users[0].Id.ToString()) })));
            _controller.ControllerContext.HttpContext = contextMock.Object;
        }


        [Fact]
        public void TestGetJobs_OneAnyUser()
        {
            var okResult = _controller.GetJobs(new GetJobsQueryObject { Index = 0, Count = 1 }) as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var resultObject = okResult.Value as GetJobsResponseObject;
            Assert.NotNull(resultObject);
            Assert.NotNull(resultObject.jobs);
            Assert.True(resultObject.jobs.Length == 1);

            var job = resultObject.jobs[0];
            Assert.Equal("Job1", job.Name);
            Assert.Equal("Job1Desc", job.Description);
            Assert.Equal(new DateTime(2020, 3, 20), job.PostDate);
            Assert.False(job.Done);
        }

        [Fact]
        public void TestGetJobs_ThreeJobsLoggedInUser()
        {
            var okResult = _controller.GetJobs(new GetJobsQueryObject
            {
                Index = 0,
                Count = 3,
                ByCurrentUserOnly = true
            }) as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var resultObject = okResult.Value as GetJobsResponseObject;
            Assert.NotNull(resultObject);
            Assert.NotNull(resultObject.jobs);
            Assert.True(resultObject.jobs.Length == 3);
        }

        [Fact]
        public void TestGetJobs_TwoJobsAllUsersInTimeSpanOrderByDate()
        {
            var okResult = _controller.GetJobs(new GetJobsQueryObject
            {
                Index = 0,
                Count = 2,
                NewerThan = new DateTime(2021, 2, 1),
                OlderThan = new DateTime(2021, 2, 2),
                OrderBy = new GetJobsQueryObject.OrderField[] { GetJobsQueryObject.OrderField.PostDate },
                Ascending = new bool[] { false }
            }) as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var resultObject = okResult.Value as GetJobsResponseObject;
            Assert.NotNull(resultObject);
            Assert.NotNull(resultObject.jobs);
            Assert.True(resultObject.jobs.Length == 2);

            Assert.Equal("Job4", resultObject.jobs[0].Name);
            Assert.Equal("Job2", resultObject.jobs[1].Name);
        }

        [Fact]
        public void TestGetJobs_FourJobsFilterByDone()
        {
            var okResult = _controller.GetJobs(new GetJobsQueryObject
            {
                Index = 0,
                Count = 4,
                FilterFields = new GetJobsQueryObject.FilterField[] { GetJobsQueryObject.FilterField.Done },
                FilterValues = new string[] { "true" },
                ExactFilters = new bool[] { true },
            }) as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var resultObject = okResult.Value as GetJobsResponseObject;
            Assert.NotNull(resultObject);
            Assert.NotNull(resultObject.jobs);
            Assert.True(resultObject.jobs.Length == 2);

            Assert.Equal("Job2", resultObject.jobs[0].Name);
            Assert.Equal("Job4", resultObject.jobs[1].Name);
        }

        [Fact]
        public void TestGetJobs_SixJobsOrderByNameAscThenDateDesc()
        {
            var okResult = _controller.GetJobs(new GetJobsQueryObject
            {
                Index = 0,
                Count = 6,
                OrderBy = new GetJobsQueryObject.OrderField[]
                {
                    GetJobsQueryObject.OrderField.Name,
                    GetJobsQueryObject.OrderField.PostDate
                },
                Ascending = new bool[] { true, false },
            }) as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var resultObject = okResult.Value as GetJobsResponseObject;
            Assert.NotNull(resultObject);
            Assert.NotNull(resultObject.jobs);
            Assert.True(resultObject.jobs.Length == 6);

            Assert.Equal("Job5", resultObject.jobs[4].Name);
            Assert.Equal("Job5", resultObject.jobs[5].Name);

            Assert.Equal(new DateTime(2020, 3, 22), resultObject.jobs[4].PostDate);
            Assert.Equal(new DateTime(2020, 3, 21), resultObject.jobs[5].PostDate);
        }

        [Fact]

        public void TestGetJobDetails_ValidJob()
        {
            _jobProvider.Setup(x => x.GetJobById(1)).Returns(new JobDAO { Id = 1, Name = "Job2", Description = "Job2Desc", Images = new JobImageDAO[0] });
            var okResult = _controller.GetJobDetails(new GetJobDetailsQueryObject { Id = 1 }) as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var resultObject = okResult.Value as JobResponseObject;
            Assert.NotNull(resultObject);

            Assert.Equal("Job2", resultObject.Name);
            Assert.Equal("Job2Desc", resultObject.Description);


        }
    }
}
