using Degano_API.Data;
using Degano_API.Models.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.Specifications;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext _appContext;

        public SubscriptionRepository(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<Subscription> AddSubscriptionAsync(Subscription Subscription)
        {
            await _appContext.Subscriptions.AddAsync(Subscription);

            return Subscription;
        }

        public async Task<Subscription?> GetSubscriptionAsync(Guid userId, Guid offerId)
        {
            return await _appContext.Subscriptions.Where(x => x.UserId == userId && x.OfferId == offerId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync()
        {
            return await _appContext.Subscriptions.ToListAsync();
        }

        public async Task<bool> SubscriptionExistsAsync(Guid userId, Guid offerId)
        {
            return await _appContext.Subscriptions.AnyAsync(x => x.UserId == userId && x.OfferId == offerId);
        }


        public Subscription RemoveSubscription(Subscription Subscription)
        {
            _appContext.Subscriptions.Remove(Subscription);
            return Subscription;
        }

        public Subscription UpdateSubscription(Subscription Subscription)
        {
            _appContext.Entry(Subscription).State = EntityState.Modified;

            return Subscription;
        }

        public async Task<Subscription?> GetSubscriptionAsync(Expression<Func<Subscription, bool>> predicate)
        {
            return await _appContext.Subscriptions.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync(ISpecification<Subscription> spec)
        {
            var res = spec.Includes
                .Aggregate(_appContext.Subscriptions.AsQueryable(),
                    (current, include) => current.Include(include));

            res = spec.IncludeStrings
                .Aggregate(res,
                    (current, include) => current.Include(include));


            if (spec.IsPagingEnabled)
            {
                res = res.Skip(spec.Skip)
                             .Take(spec.Take);
            }

            if (spec.Criteria != null)
            {
                res = res.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                res = res.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                res = res.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.GroupBy != null)
            {
                res = res.GroupBy(spec.GroupBy).SelectMany(x => x);
            }

            return await res.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _appContext.SaveChangesAsync();
        }

    }
}
