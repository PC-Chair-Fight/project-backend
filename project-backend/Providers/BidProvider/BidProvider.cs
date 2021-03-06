using project_backend.Models.Bid;
using project_backend.Models.Exceptions;
using project_backend.Models.Worker;
using project_backend.Repos;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace project_backend.Providers.BidProvider
{
    public class BidProvider : IBidProvider
    {

        private readonly DatabaseContext _dbContext;

        public BidProvider(DatabaseContext databaseContext) =>
            _dbContext = databaseContext;

        public BidDAO AddBid(float sum, int workerId, int jobId)
        {
            WorkerDAO worker = _dbContext.Workers.Find(workerId);

            if (worker == null)
            {
                throw new NotQualifiedException();
            }

            BidDAO newBid = new BidDAO
            {
                Sum = sum,
                WorkerId = workerId,
                JobId = jobId
            };

            _dbContext.Bids.Add(newBid);
            _dbContext.SaveChanges();

            return newBid;
        }

        public BidDAO EditBid(float sum, int workerId, int bidId)
        {
            BidDAO currentBid;
            try
            {
                currentBid = _dbContext.Bids.Where((bid) => bid.Id == bidId && bid.WorkerId == workerId).First();
            }
            catch (InvalidOperationException)
            {
                throw new ResourceNotFoundException("Bid not found");
            }

            BidDAO editedBid = currentBid;
            editedBid.Sum = sum;

            _dbContext.Bids.Update(editedBid);
            _dbContext.SaveChanges();
            return editedBid;

        }

        public IQueryable<BidDAO> QueryJobBids(int jobId)
        {
            return _dbContext.Bids.Include(bid => bid.Worker).ThenInclude(worker => worker.User).Where(b => b.JobId == jobId).OrderBy(bid => bid.Sum).AsQueryable();

            //return _dbContext.Bids.Include(bid => bid.Worker).Join(_dbContext.Users,bid => bid.WorkerId, user => user.Id,res => res.).Where(b => b.JobId == jobId).OrderBy(bid => bid.Sum).AsQueryable();
        }
    }
}
