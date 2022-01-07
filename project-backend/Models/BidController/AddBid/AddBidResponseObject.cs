namespace project_backend.Models.BidController.AddBid
{
    public class AddBidResponseObject
    {
        public int Id { get; set; }
        public float Sum { get; set; }
        public int JobId { get; set; }
        public int WorkerId { get; set; }
    }
}
