using Degano_API.Models.DTOs.Response;

namespace Degano_API.Models.DTOs.Request
{
    public class OfferDTORequest
    {
        public int DurationInDays { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime TimeUntilOfferExpiration { get; set; }

        public OfferDTORequest (int durationInDays, double price, int discount, bool isActive, DateTime startTime, DateTime timeUntilOfferExpiration)
        {
            DurationInDays = durationInDays;
            Price = price;
            Discount = discount;
            IsActive = isActive;
            StartTime = startTime;
            TimeUntilOfferExpiration = timeUntilOfferExpiration;
        }

        public OfferDTORequest() { }
    }
}
