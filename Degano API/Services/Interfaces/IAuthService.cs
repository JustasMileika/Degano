using Domain.DTOs.Request;

namespace Degano_API.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<string> Login(UserLoginDTO userLogin);
    }
}
