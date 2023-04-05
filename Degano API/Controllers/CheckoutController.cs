using Degano_API.Models.DTOs.Response;
using Degano_API.Models.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Models.Specifications;
using Stripe;
using Stripe.Checkout;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Degano_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class CheckoutController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IOfferRepository _offerRepository;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IUserRepository _userRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;

    const string endpointSecret = "whsec_649c86eed33a42cf9ffcd28c46c7e779f5fdcdfdcab235caedf16968fd237b38";

    private static string s_wasmClientURL = string.Empty;

    public CheckoutController(IConfiguration configuration, IOfferRepository offerRepository,
        IHttpContextAccessor httpContext, IUserRepository userRepository, ISubscriptionRepository subscriptionRepository)
    {
        _configuration = configuration;
        _offerRepository = offerRepository;
        _httpContext = httpContext;
        _userRepository = userRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    [HttpPost]
    public async Task<ActionResult> Checkout([FromBody] Guid offerId, [FromServices] IServiceProvider sp)
    {

        var userId = Guid.Parse(((ClaimsIdentity)_httpContext.HttpContext.User.Identity).FindFirst("Id").Value);

        //var referer = Request.Headers.Referer;
        //s_wasmClientURL = referer[0];

        // Build the URL to which the customer will be redirected after paying.
        var server = sp.GetRequiredService<IServer>();

        var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

        string? thisApiUrl = null;

        if (serverAddressesFeature is not null)
        {
            thisApiUrl = serverAddressesFeature.Addresses.FirstOrDefault();
        }

        if (thisApiUrl is not null)
        {
            var sessionUrl = await CheckOut(offerId, userId);
            var pubKey = _configuration["Stripe:PubKey"];

            var checkoutOrderResponse = new CheckoutDTOResponse()
            {
                SessionUrl = sessionUrl,
                PubKey = pubKey
            };

            return Ok(checkoutOrderResponse.SessionUrl);
        }
        else
        {
            return StatusCode(500);
        }
    }

    [NonAction]
    public async Task<string> CheckOut(Guid offerId, Guid userId)
    {
        // Create a payment flow from the items in the cart.
        // Gets sent to Stripe API.

        
        var spec = new UserSubscriptionSpecification(userId);

        var mostRecentSub = (await _subscriptionRepository.GetSubscriptionsAsync(spec)).FirstOrDefault();

        bool isPremium = false;

        if (mostRecentSub != null)
        {
            if ((DateTime.Now - mostRecentSub.OrderDate).TotalDays < mostRecentSub.Offer.DurationInDays)
            {
                isPremium = true;
            }
        }

        if (isPremium)
            throw new Exception("User already has premium");

        var offer = await _offerRepository.GetOfferAsync(offerId);

        if(offer is null)
        {
            throw new Exception("No such offer");
        }

        string offerStripeId = null;

        if(offer.StripeId is not null)
        {
            offerStripeId = offer.StripeId;
        }
        else
        {
            var offerOptions = new ProductCreateOptions
            {
                Name = "Premium"
            };

            var offerService = new ProductService();

            Product stripeOffer = null;

            try
            {
                stripeOffer = offerService.Create(offerOptions);
            }
            catch(Exception e)
            {

            }
            

            offer.StripeId = stripeOffer.Id;
            await _offerRepository.SaveChangesAsync();

            offerStripeId = offer.StripeId;
        }

        var user = await _userRepository.GetUserAsync(userId);

        await _userRepository.SaveChangesAsync();

        if(user is null)
            throw new Exception("No such user in database");



        string userStripeId = null;

        if(user.StripeId is not null)
        {
            userStripeId = user.StripeId;
        }
        else
        {
            var accOptions = new CustomerCreateOptions
            {
                //Password = "hello",
                Email = user.Email,
                //Description = "My First Test Customer (created for API docs at https://www.stripe.com/docs/api)",
            };
            var accService = new CustomerService();

            
            var customer = accService.Create(accOptions);

            try
            {
                user.StripeId = customer.Id;
                await _userRepository.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }

           

            userStripeId = customer.Id;

        }


        


        
        var options = new SessionCreateOptions
        {
            // Stripe calls the URLs below when certain checkout events happen such as success and failure.
            //SuccessUrl = $"{thisApiUrl}/checkout/success?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
            //SuccessUrl = "degano://stripe-redirect",
            SuccessUrl = "https://10.0.2.2:7253/api/checkout/success",
            CancelUrl = "https://10.0.2.2:7253/api/checkout/failure",
            //CancelUrl = s_wasmClientURL + "failed",  // Checkout cancelled.
            PaymentMethodTypes = new List<string> // Only card available in test mode?
            {
                "card"
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (int)(offer.Price * 100), // Price is in USD cents.
                        Currency = "EUR",
                        /*ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            
                            Name = "Premium",
                            //Description = product.Description,
                            //Images = new List<string> { product.ImageUrl }
                        },*/
                        Product = offerStripeId,
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment", // One-time payment. Stripe supports recurring 'subscription' payments.

            Customer = userStripeId,
            
            
        };

        Session session = null;

        try
        {
            var service = new SessionService();
            session = await service.CreateAsync(options);
        }
        catch(Exception e)
        {

        }

        //session.Customer.Id = userId.ToString();

        return session.Url;
    }


    [HttpGet("success")]
    public async Task<ActionResult> RedirectToSuccess()
    {
        return Redirect("degano://success");
    }

    [HttpGet("failure")]
    public async Task<ActionResult> RedirectToFailure()
    {
        return Redirect("degano://failure");
    }


    [HttpPost("webhook")]
    public async Task<ActionResult> SuccessfullPaymentHandler()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret);

            if(stripeEvent.Type == Events.CheckoutSessionCompleted)
            {

                var checkoutSession = stripeEvent.Data.Object as Stripe.Checkout.Session;
                var customerID = checkoutSession.CustomerId;
                

                Console.WriteLine("customerId=" + customerID);
                var options = new SessionListLineItemsOptions
                {
                    Limit = 1
                };
                var service = new SessionService();
                StripeList<LineItem> lineItems = service.ListLineItems(checkoutSession.Id, options);

                var productId = lineItems.Data.ToArray()[0].Price.ProductId;

                Console.WriteLine("productId=" + lineItems.Data.ToArray()[0].Price.ProductId);

                var user = await _userRepository.GetUserAsync(user => user.StripeId == customerID);
                var offer = await _offerRepository.GetOfferAsync(offer => offer.StripeId == productId);


                if (user != null && offer != null)
                {
                    await _subscriptionRepository.AddSubscriptionAsync(new Models.Entities.Subscription(
                        user.Id,
                        offer.Id,
                        DateTime.Now
                        ));
                    await _subscriptionRepository.SaveChangesAsync();
                }
            }
            

            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }

    }
}
