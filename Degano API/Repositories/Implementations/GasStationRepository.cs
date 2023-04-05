using Degano_API.Data;
using Degano_API.Models.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.Specifications;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class GasStationRepository : IGasStationRepository
    {
        private readonly AppDbContext _appContext;

        public GasStationRepository(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<GasStation> AddGasStationAsync(GasStation gasStation)
        {
            await _appContext.GasStations.AddAsync(gasStation);

            return gasStation;
        }

        public async Task<GasStation?> GetGasStationAsync(Guid id)
        {
            return await _appContext.GasStations.FindAsync(id);
        }

        public async Task<IEnumerable<GasStation>> GetGasStationsAsync()
        {
            return await _appContext.GasStations.ToListAsync();
        }

        public async Task<bool> GasStationExistsAsync(Guid id)
        {
            //return await _appContext.GasStations.AnyAsync(e => e.Id == id);
            throw new NotImplementedException();
        }


        public GasStation RemoveGasStation(GasStation gasStation)
        {
            _appContext.GasStations.Remove(gasStation);
            return gasStation;
        }

        public GasStation UpdateGasStation(GasStation gasStation)
        {
            _appContext.Entry(gasStation).State = EntityState.Modified;

            return gasStation;
        }

        public async Task<GasStation?> GetGasStationAsync(Expression<Func<GasStation, bool>> predicate)
        {
            return await _appContext.GasStations.FirstOrDefaultAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await _appContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<GasStation>> GetGasStationsAsync(ISpecification<GasStation> spec)
        {
            var res = spec.Includes
                .Aggregate(_appContext.GasStations.AsQueryable(),
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
