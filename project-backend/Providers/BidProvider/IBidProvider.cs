using project_backend.Models.Bid;
using System.Linq;

namespace project_backend.Providers.BidProvider
{
    public interface IBidProvider
    {
        public BidDAO AddBid(float sum, int workerId, int jobId);
        public BidDAO EditBid(float sum, int workerId, int bidId);
        public IQueryable<BidDAO> QueryJobBids(int jobId);

    }
}
