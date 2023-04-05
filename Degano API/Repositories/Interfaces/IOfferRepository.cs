using Degano_API.Models.Entities;
using Models.Specifications;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories
{
    public interface IOfferRepository
    {
        public Task<Offer?> GetOfferAsync(Guid id);

        public Task<IEnumerable<Offer>> GetOffersAsync();
        
        public Offer RemoveOffer(Offer Offer);

        public Offer UpdateOffer(Offer Offer);

        public Task<Offer> AddOfferAsync(Offer Offer);

        public Task<bool> OfferExistsAsync(Guid id);

        public Task<Offer?> GetOfferAsync(Expression<Func<Offer, bool>> predicate);

        public Task<IEnumerable<Offer>> GetOffersAsync(ISpecification<Offer> spec);

        public Task SaveChangesAsync();
    }
}
