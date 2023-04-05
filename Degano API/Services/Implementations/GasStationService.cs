using Degano_API.Models.DTOs.Response;
using Degano_API.Services.Interfaces;
using Domain.Interfaces.Repositories;
using System.Security.Claims;

namespace Degano_API.Services.Implementations
{
    public class GasStationService : IGasStationService
    {
        private readonly IGasStationRepository _gasStationRepository;
        private readonly IHttpContextAccessor _httpContext;

        public GasStationService(IGasStationRepository gasStationRepository, IHttpContextAccessor httpContext)
        {
            _gasStationRepository = gasStationRepository;
            _httpContext = httpContext;
        }

        public async Task<IEnumerable<GasStationDTOResponse>> GetGasStations()
        {

            string? userRole = null;
            try
            {
                userRole = ((ClaimsIdentity)_httpContext.HttpContext.User.Identity).FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;
            }
            catch(Exception)
            {

            }


            

            var gasStations = await _gasStationRepository.GetGasStationsAsync();

            var gasStationsDTO = new List<GasStationDTOResponse>();

            if(userRole == "basic" || userRole is null)
            {
                foreach (var gasStation in gasStations)
                {
                    if(gasStation.DailyUpdateTimestamp != null)
                    {
                        var gasStationDTO = new GasStationDTOResponse(
                        gasStation.Id,
                        gasStation.Name,
                        gasStation.Address,
                        gasStation.Location.Latitude,
                        gasStation.Location.Longitude,
                        gasStation.FuelPriceDaily.price95,
                        gasStation.FuelPriceDaily.price98,
                        gasStation.FuelPriceDaily.priceDiesel,
                        gasStation.FuelPriceDaily.priceLPG,
                        gasStation.Type,
                        (DateTime)gasStation.DailyUpdateTimestamp
                        );
                        gasStationsDTO.Add(gasStationDTO);
                    }
                }
            }
            else if(userRole == "premium")
            {
                foreach (var gasStation in gasStations)
                {
                    var gasStationDTO = new GasStationDTOResponse(
                    gasStation.Id,
                    gasStation.Name,
                    gasStation.Address,
                    gasStation.Location.Latitude,
                    gasStation.Location.Longitude,
                    gasStation.FuelPriceHourly.price95,
                    gasStation.FuelPriceHourly.price98,
                    gasStation.FuelPriceHourly.priceDiesel,
                    gasStation.FuelPriceHourly.priceLPG,
                    gasStation.Type,
                    gasStation.HourlyUpdateTimestamp
                    );

                    gasStationsDTO.Add(gasStationDTO);
                }
            }
            else
            {
                // should never happen
            }

            

            return gasStationsDTO;
        }
    }
}
