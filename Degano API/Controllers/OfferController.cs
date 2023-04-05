using Degano_API.Models.DTOs.Request;
using Degano_API.Models.DTOs.Response;
using Degano_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Degano_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _OfferService;

        public OfferController(IOfferService OfferService)
        {
            _OfferService = OfferService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfferDTOResponse>>> GetOffers()
        {
            return Ok(await _OfferService.GetOffers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OfferDTOResponse>> GetOffer(Guid id)
        {
            return Ok(await _OfferService.GetOffer(id));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Offer")]
        public async Task<ActionResult<OfferDTOResponse>> PutOffer(Guid id, OfferDTORequest Offer)
        {
            return Ok(await _OfferService.PutOffer(id, Offer));
        }

        [HttpPost]
        public async Task<ActionResult<OfferDTOResponse>> PostOffer(OfferDTORequest Offer)
        {
            return Ok(await _OfferService.PostOffer(Offer));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Offer")]
        public async Task<ActionResult<OfferDTOResponse>> DeleteOffer(Guid id)
        {
            return Ok(await _OfferService.DeleteOffer(id));
        }
    }
}
