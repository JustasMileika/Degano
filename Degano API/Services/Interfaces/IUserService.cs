using Degano_API.Models.DTOs.Request;
using Degano_API.Models.DTOs.Response;

namespace Degano_API.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTOResponse>> GetUsers();

        Task<UserDTOResponse> GetUser(Guid id);

        Task<UserDTOResponse> PutUser(Guid id, UserDTORequest User);

        Task<UserDTOResponse> DeleteUser(Guid id);

        Task<UserDTOResponse> PostUser(UserDTORequest User);
    }
}
