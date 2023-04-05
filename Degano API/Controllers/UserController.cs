using Degano_API.Models.DTOs.Request;
using Degano_API.Models.DTOs.Response;
using Degano_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Degano_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService UserService)
        {
            _UserService = UserService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTOResponse>>> GetUsers()
        {
            return Ok(await _UserService.GetUsers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTOResponse>> GetUser(Guid id)
        {
            return Ok(await _UserService.GetUser(id));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserDTOResponse>> PutUser(Guid id, UserDTORequest User)
        {
            return Ok(await _UserService.PutUser(id, User));
        }

        [HttpPost]
        public async Task<ActionResult<UserDTOResponse>> PostUser(UserDTORequest User)
        {
            return Ok(await _UserService.PostUser(User));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserDTOResponse>> DeleteUser(Guid id)
        {
            return Ok(await _UserService.DeleteUser(id));
        }
    }
}
