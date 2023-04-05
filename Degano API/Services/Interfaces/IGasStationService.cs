using Degano_API.Models.DTOs.Response;
using Domain.DTOs.Request;

namespace Degano_API.Services.Interfaces
{
    public interface IGasStationService
    {
        public Task<IEnumerable<GasStationDTOResponse>> GetGasStations();
    }
}
