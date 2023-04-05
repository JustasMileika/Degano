
using Degano_API.Models.DTOs.Request;
using Degano_API.Models.DTOs.Response;
using Degano_API.Models.Entities;
using Degano_API.Services.Interfaces;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using System.Security.Claims;

namespace Degano_API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;
        private readonly IHttpContextAccessor _httpContext;

        public UserService(IHttpContextAccessor httpContext,
            IUserRepository UserRepository)
        {
            _UserRepository = UserRepository;
            _httpContext = httpContext;
        }

        public async Task<UserDTOResponse> DeleteUser(Guid id)
        {
            if (id !=
               Guid.Parse(((ClaimsIdentity)_httpContext.HttpContext.User.Identity).FindFirst("Id").Value))
                throw new InvalidIdentityException("you are unauthorized to delete this recource");

            var User = await _UserRepository.GetUserAsync(id);
            if (User == null)
            {
                throw new RecourseNotFoundException("User with this id does not exist");
            }

            _UserRepository.RemoveUser(User);
            await _UserRepository.SaveChangesAsync();

            var UserResponse = new UserDTOResponse(
                User.Id
                );

            return UserResponse;
        }


        public async Task<UserDTOResponse> GetUser(Guid id)
        {
            var User = await _UserRepository.GetUserAsync(id);

            if (User == null)
            {
                throw new RecourseNotFoundException("User with this id does not exist");
            }
            else
            {
                var UserResponse = new UserDTOResponse(
                User.Id
                );
                return UserResponse;
            }
        }

        public async Task<IEnumerable<UserDTOResponse>> GetUsers()
        {
            var Users = await _UserRepository.GetUsersAsync();
            var UsersResponse = new List<UserDTOResponse>();
            foreach (var User in Users)
            {
                var UserResponse = new UserDTOResponse(
                User.Id
                );
                UsersResponse.Add(UserResponse);
            }
            return UsersResponse;
        }

        public async Task<UserDTOResponse> PostUser(UserDTORequest userToPost)
        {
            if (await _UserRepository.GetUserAsync(
                User => User.Email == userToPost.Email) != null)
            {
                throw new RecourseAlreadyExistsException("User with this email already exists");
            }

            var id = Guid.NewGuid();

            var user = new User(
                id,
                userToPost.Email,
                userToPost.Password);
                



            await _UserRepository.AddUserAsync(user);
            await _UserRepository.SaveChangesAsync();


            var UserResponse = new UserDTOResponse(
                user.Id
            );

            return UserResponse;
        }

        public async Task<UserDTOResponse> PutUser(Guid id, UserDTORequest userToUpdate)
        {
            if (id !=
                Guid.Parse(((ClaimsIdentity)_httpContext.HttpContext.User.Identity).FindFirst("Id").Value))
                throw new InvalidIdentityException("you are unauthorized to update this resource");

            if (!await _UserRepository.UserExistsAsync(id))
            {
                throw new RecourseNotFoundException("User with this id does not exist");
            }

            if (await _UserRepository.GetUserAsync(
                user => user.Email == userToUpdate.Email) != null)
            {
                throw new RecourseAlreadyExistsException("User with this email already exists");
            }



            var user = new User(
                id,
                userToUpdate.Email!,
                userToUpdate.Password!
                );
                

            _UserRepository.UpdateUser(user);
            await _UserRepository.SaveChangesAsync();



            var userResponse = new UserDTOResponse(
                user.Id
                );

            return userResponse;
        }
    }
}
