using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using project_backend.Controllers;
using project_backend.Models.Job;
using project_backend.Models.JobController.GetJobs;
using project_backend.Models.User;
using project_backend.Providers.JobProvider;
using project_backend.Repos;
using System;
using System.Reflection;
using System.Security.Claims;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace project_backend.Test.ControllerTests
{
    [Trait("Category", "Unit")]
    public class JobControllerTest : IDisposable
    {
        private readonly JobController _controller;
        private readonly DatabaseContext _dbContext;

        public JobControllerTest(ITestOutputHelper output)
        {
            var type = output.GetType();
            var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            var test = (ITest)testMember.GetValue(output);

            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(test.DisplayName) // use the name of the test, because otherwise the db is used simultaneously by multiple threads
                .Options;
            _dbContext = new DatabaseContext(options);

            var user1 = new UserDAO
            {
                Email = "user1@gmail.com",
                DateOfBirth = new DateTime(2020, 2, 20),
                FirstName = "User1",
                LastName = "User1",
                Password = ""
            };

            var user2 = new UserDAO
            {
                Email = "user2@gmail.com",
                DateOfBirth = new DateTime(2021, 1, 1),
                FirstName = "User2",
                LastName = "User2",
                Password = ""
            };

            _dbContext.Users.AddRange(user1, user2);
            _dbContext.SaveChanges();

            _dbContext.Jobs.AddRange(
                    new JobDAO
                    {
                        Name = "Job1",
                        Description = "Job1Desc",
                        User = user1,
                        PostDate = new DateTime(2020, 3, 20),
                        Done = false,
                    },
                    new JobDAO
                    {
                        Name = "Job2",
                        Description = "Job2Desc",
                        User = user2,
                        PostDate = new DateTime(2021, 2, 1),
                        Done = true,
                    },
                    new JobDAO
                    {
                        Name = "Job3",
                        Description = "Job3Desc",
                        User = user1,
                        PostDate = new DateTime(2020, 3, 21),
                        Done = false,
                    },
                    new JobDAO
                    {
                        Name = "Job4",
                        Description = "Job4Desc",
                        User = user2,
                        PostDate = new DateTime(2021, 2, 2),
                        Done = true,
                    },
                    new JobDAO
                    {
                        Name = "Job5",
                        Description = "Job5Desc",
                        User = user1,
                        PostDate = new DateTime(2020, 3, 21),
                        Done = false,
                    },
                    new JobDAO
                    {
                        Name = "Job5",
                        Description = "Job5Desc",
                        User = user1,
                        PostDate = new DateTime(2020, 3, 22),
                        Done = false,
                    }
                );
            _dbContext.SaveChanges();

            _controller = new JobController(NullLogger<JobController>.Instance, new JobProvider(_dbContext));

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("Id", user1.Id.ToString()) })));
            _controller.ControllerContext.HttpContext = contextMock.Object;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
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
    }
}
