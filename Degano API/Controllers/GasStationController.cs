using Degano_API.Models.DTOs.Response;
using Degano_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Degano_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GasStationController : ControllerBase
    {
        private readonly IGasStationService _gasStationService;

        public GasStationController(IGasStationService gasStationService)
        {
            _gasStationService = gasStationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GasStationDTOResponse>>> GetGasStations()
        {
            return Ok(await _gasStationService.GetGasStations());
        }
    }
}
