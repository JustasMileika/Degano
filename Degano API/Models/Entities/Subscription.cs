namespace Degano_API.Models.Entities
{
    public class Subscription
    {

        public Subscription(Guid userId, Guid offerId, DateTime orderDate)
        {
            UserId = userId;
            OfferId = offerId;
            OrderDate = orderDate;
        }

        public Guid UserId { get; set; }

        public User? User { get; set; }

        public Guid OfferId { get; set; }

        public Offer? Offer { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
