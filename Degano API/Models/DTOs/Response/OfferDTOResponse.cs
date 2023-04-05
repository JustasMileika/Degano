namespace Degano_API.Models.DTOs.Response
{
    public class OfferDTOResponse
    {
        public Guid Id { get; set; }

        public string? StripeId { get; set; }

        public int DurationInDays { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime TimeUntilOfferExpiration { get; set; }

        public OfferDTOResponse(Guid id, string? stripeId, int durationInDays, double price, int discount, bool isActive, DateTime startTime, DateTime timeUntilOfferExpiration)
        {
            Id = id;
            StripeId = stripeId;
            DurationInDays = durationInDays;
            Price = price;
            Discount = discount;
            IsActive = isActive;
            StartTime = startTime;
            TimeUntilOfferExpiration = timeUntilOfferExpiration;
        }
    }
}
