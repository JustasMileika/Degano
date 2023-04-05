using Degano_API.Models.Entities;
using Models.Specifications;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories
{
    public interface ISubscriptionRepository
    {
        public Task<Subscription?> GetSubscriptionAsync(Guid userId, Guid offerId);

        public Task<IEnumerable<Subscription>> GetSubscriptionsAsync();
        
        public Subscription RemoveSubscription(Subscription Subscription);

        public Subscription UpdateSubscription(Subscription Subscription);

        public Task<Subscription> AddSubscriptionAsync(Subscription Subscription);

        public Task<bool> SubscriptionExistsAsync(Guid userId, Guid offerId);

        public Task<Subscription?> GetSubscriptionAsync(Expression<Func<Subscription, bool>> predicate);

        public Task<IEnumerable<Subscription>> GetSubscriptionsAsync(ISpecification<Subscription> spec);

        public Task SaveChangesAsync();
    }
}
