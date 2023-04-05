using Degano_API.Models.Entities;
using Models.Specifications;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories
{
    public interface IGasStationRepository
    {
        public Task<GasStation?> GetGasStationAsync(Guid id);

        public Task<IEnumerable<GasStation>> GetGasStationsAsync();
        
        public GasStation RemoveGasStation(GasStation gasStation);

        public GasStation UpdateGasStation(GasStation gasStation);

        public Task<GasStation> AddGasStationAsync(GasStation GasSgasStationtation);

        public Task<bool> GasStationExistsAsync(Guid id);

        public Task<GasStation?> GetGasStationAsync(Expression<Func<GasStation, bool>> predicate);

        public Task<IEnumerable<GasStation>> GetGasStationsAsync(ISpecification<GasStation> spec);

        public Task SaveChangesAsync();
    }
}
