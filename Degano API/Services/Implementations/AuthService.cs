using Degano_API.Models.Entities;
using Degano_API.Services.Interfaces;
using Domain.DTOs.Request;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Microsoft.IdentityModel.Tokens;
using Models.Specifications;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Degano_API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public AuthService(IConfiguration config, IUserRepository userRepository,
            ISubscriptionRepository subscriptionRepository)
        {
            _config = config;
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<string> Login(UserLoginDTO userLogin)
        {
            var user = await AuthenticateUserAsync(userLogin);

            var spec = new UserSubscriptionSpecification(user.Id);

            var mostRecentSub = (await _subscriptionRepository.GetSubscriptionsAsync(spec)).FirstOrDefault();

            bool isPremium = false;

            if(mostRecentSub != null)
            {
                if ((DateTime.Now - mostRecentSub.OrderDate).TotalDays < mostRecentSub.Offer.DurationInDays)
                {
                    isPremium = true;
                }
            }

            var token = GenerateToken(user, isPremium);
            return token;
        }

        public async Task<string> RegenerateJwt(UserLoginDTO userLogin)
        {
            var user = await AuthenticateUserAsync(userLogin);

            var spec = new UserSubscriptionSpecification(user.Id);

            var mostRecentSub = (await _subscriptionRepository.GetSubscriptionsAsync(spec)).FirstOrDefault();

            bool isPremium = false;

            if (mostRecentSub != null)
            {
                if ((DateTime.Now - mostRecentSub.OrderDate).TotalDays < mostRecentSub.Offer.DurationInDays)
                {
                    isPremium = true;
                }
            }

            var token = GenerateToken(user, isPremium);
            return token;
        }

        public string GenerateToken(User user, bool isPremium)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, isPremium ? "premium" : "basic")
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> AuthenticateUserAsync(UserLoginDTO userLogin)
        {
            var user = await _userRepository.GetUserAsync(user => user.Email.ToLower() == userLogin.Email!.ToLower());


            if (user != null)
            {
                if (user.Password.ToLower() == userLogin.Password!.ToLower())
                    return user;
                else
                    throw new InvalidLoginCredentialsException("incorrect password");
            }
            else
            {
                throw new InvalidLoginCredentialsException("no user with this email exists");
            }
        }
    }
}
