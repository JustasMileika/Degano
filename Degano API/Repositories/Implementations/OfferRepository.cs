using Degano_API.Data;
using Degano_API.Models.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.Specifications;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly AppDbContext _appContext;

        public OfferRepository(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<Offer> AddOfferAsync(Offer Offer)
        {
            await _appContext.Offers.AddAsync(Offer);

            return Offer;
        }

        public async Task<Offer?> GetOfferAsync(Guid id)
        {
            return await _appContext.Offers.FindAsync(id);
        }

        public async Task<IEnumerable<Offer>> GetOffersAsync()
        {
            return await _appContext.Offers.ToListAsync();
        }

        public async Task<bool> OfferExistsAsync(Guid id)
        {
            return await _appContext.Offers.AnyAsync(e => e.Id == id);
        }


        public Offer RemoveOffer(Offer Offer)
        {
            _appContext.Offers.Remove(Offer);
            return Offer;
        }

        public Offer UpdateOffer(Offer Offer)
        {
            _appContext.Entry(Offer).State = EntityState.Modified;

            return Offer;
        }

        public async Task<Offer?> GetOfferAsync(Expression<Func<Offer, bool>> predicate)
        {
            return await _appContext.Offers.FirstOrDefaultAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await _appContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Offer>> GetOffersAsync(ISpecification<Offer> spec)
        {
            var res = spec.Includes
                .Aggregate(_appContext.Offers.AsQueryable(),
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
    }
}
