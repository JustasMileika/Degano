using Degano_API.Models.DTOs.Request;
using Degano_API.Models.DTOs.Response;

namespace Degano_API.Services.Interfaces
{
    public interface IOfferService
    {
        Task<IEnumerable<OfferDTOResponse>> GetOffers();

        Task<OfferDTOResponse> GetOffer(Guid id);

        Task<OfferDTOResponse> PutOffer(Guid id, OfferDTORequest offer);

        Task<OfferDTOResponse> DeleteOffer(Guid id);

        Task<OfferDTOResponse> PostOffer(OfferDTORequest offer);
    }
}
