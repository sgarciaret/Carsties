using MongoDB.Entities;

namespace BiddingService.Models
{
    public class Auction: Entity
    {
        public DateTime AuctionEnd { get; set; }
        public string Seller { get; set; }
        public int ReservePrize { get; set; }
        public bool Finished { get; set; }
    }
}
