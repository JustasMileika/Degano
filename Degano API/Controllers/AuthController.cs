using Degano_API.Services.Interfaces;
using Domain.DTOs.Request;
using Microsoft.AspNetCore.Mvc;

namespace Degano_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDTO userLogin)
        {
            return Ok(await _authService.Login(userLogin));
        }

        [HttpPost("regenerateJwt")]
        public async Task<ActionResult<string>> RegenerateJwt(UserLoginDTO userLogin)
        {
            return Ok(await _authService.Login(userLogin));
        }
    }
}
