
using Degano_API.Models.DTOs.Request;
using Degano_API.Models.DTOs.Response;
using Degano_API.Models.Entities;
using Degano_API.Services.Interfaces;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using System;
using System.Security.Claims;

namespace Degano_API.Services.Implementations
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _OfferRepository;
        private readonly IHttpContextAccessor _httpContext;

        public OfferService(IHttpContextAccessor httpContext,
            IOfferRepository OfferRepository)
        {
            _OfferRepository = OfferRepository;
            _httpContext = httpContext;
        }

        public async Task<OfferDTOResponse> DeleteOffer(Guid id)
        {
            var offer = await _OfferRepository.GetOfferAsync(id);
            if (offer == null)
            {
                throw new RecourseNotFoundException("Offer with this id does not exist");
            }

            _OfferRepository.RemoveOffer(offer);
            await _OfferRepository.SaveChangesAsync();

            var offerResponse = new OfferDTOResponse(
                offer.Id,
                offer.StripeId,
                offer.DurationInDays,
                offer.Price,
                offer.Discount,
                offer.IsActive,
                offer.StartTime,
                offer.TimeUntilOfferExpiration
                );

            return offerResponse;
        }


        public async Task<OfferDTOResponse> GetOffer(Guid id)
        {
            var offer = await _OfferRepository.GetOfferAsync(id);

            if (offer == null)
            {
                throw new RecourseNotFoundException("Offer with this id does not exist");
            }
            else
            {
                var offerResponse = new OfferDTOResponse(
                offer.Id,
                offer.StripeId,
                offer.DurationInDays,
                offer.Price,
                offer.Discount,
                offer.IsActive,
                offer.StartTime,
                offer.TimeUntilOfferExpiration
                );
                return offerResponse;
            }
        }

        public async Task<IEnumerable<OfferDTOResponse>> GetOffers()
        {
            var offers = await _OfferRepository.GetOffersAsync();
            var offersResponse = new List<OfferDTOResponse>();
            foreach (var offer in offers)
            {
                var offerResponse = new OfferDTOResponse(
                offer.Id,
                offer.StripeId,
                offer.DurationInDays,
                offer.Price,
                offer.Discount,
                offer.IsActive,
                offer.StartTime,
                offer.TimeUntilOfferExpiration
                );
                offersResponse.Add(offerResponse);
            }
            return offersResponse;
        }

        public async Task<OfferDTOResponse> PostOffer(OfferDTORequest offerToPost)
        {
            var id = Guid.NewGuid();

            var offer = new Offer(
                id,
                null,
                offerToPost.DurationInDays,
                offerToPost.Price,
                offerToPost.Discount,
                offerToPost.IsActive,
                offerToPost.StartTime,
                offerToPost.TimeUntilOfferExpiration
                );
                

            await _OfferRepository.AddOfferAsync(offer);
            await _OfferRepository.SaveChangesAsync();


            var offerResponse = new OfferDTOResponse(
                offer.Id,
                offer.StripeId,
                offer.DurationInDays,
                offer.Price,
                offer.Discount,
                offer.IsActive,
                offer.StartTime,
                offer.TimeUntilOfferExpiration
                );

            return offerResponse;
        }

        public async Task<OfferDTOResponse> PutOffer(Guid id, OfferDTORequest offerToUpdate)
        {
            if (!await _OfferRepository.OfferExistsAsync(id))
            {
                throw new RecourseNotFoundException("Offer with this id does not exist");
            }


            var offer = new Offer(
                id,
                null,
                offerToUpdate.DurationInDays,
                offerToUpdate.Price,
                offerToUpdate.Discount,
                offerToUpdate.IsActive,
                offerToUpdate.StartTime,
                offerToUpdate.TimeUntilOfferExpiration
                );


            _OfferRepository.UpdateOffer(offer);
            await _OfferRepository.SaveChangesAsync();



            var offerResponse = new OfferDTOResponse(
                offer.Id,
                offer.StripeId,
                offer.DurationInDays,
                offer.Price,
                offer.Discount,
                offer.IsActive,
                offer.StartTime,
                offer.TimeUntilOfferExpiration
                );

            return offerResponse;
        }
    }
}
