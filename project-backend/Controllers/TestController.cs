using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using project_backend.Models.Bid;
using project_backend.Models.Job;
using project_backend.Repos;

namespace project_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private DatabaseContext _ctx;
        private IWebHostEnvironment _env;
        public TestController(IWebHostEnvironment env, DatabaseContext ctx)
        {
            _ctx = ctx;
            _env = env;
        }

        [HttpPost]
        public void CreateMockData()
        {
            if (!_env.IsDevelopment())
            {
                return;
            }

            var workerUser = new Models.User.UserDAO
            {
                FirstName = "test1",
                LastName = "last1",
                DateOfBirth = System.DateTime.Now,
                Email = "test1@yahoo.com",
                Password = "@@@"
            };
            _ctx.Users.Add(workerUser);

            var normalUser = new Models.User.UserDAO
            {
                FirstName = "test2",
                LastName = "last2",
                DateOfBirth = System.DateTime.Now,
                Email = "test2@yahoo.com",
                Password = "@@@"
            };
            _ctx.Users.Add(normalUser);

            var worker = new Models.Worker.WorkerDAO
            {
                User = workerUser,
                WorkerSince = System.DateTime.Now,
            };
            _ctx.Workers.Add(worker);

            var job = new JobDAO
            {
                Name = "test",
                Description = "test",
                PostDate = System.DateTime.Now,
                Done = false,
                User = normalUser,
            };
            _ctx.Jobs.Add(job);

            var bid = new BidDAO
            {
                Sum = 100,
                Worker = worker,
                Job = job
            };
            _ctx.Bids.Add(bid);

            _ctx.SaveChanges();
        }
    }
}
