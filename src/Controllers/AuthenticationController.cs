using KnOwl.Entities.Authentication;
using KnOwl.Models.Authentication;
using KnOwl.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KnOwl.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetAll());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var entity = await _userService.Add(user);

            if (entity == null)
                return BadRequest(new { message = "This user already exists!" });

            return Ok(new { message = "New user was created successefuly!" });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.Get(id);

            if (user == null)
                return NotFound(new { message = "User was not found!" });

            return Ok(user);
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            var response = await _userService.Authenticate(request);

            if (response == null)
                return BadRequest(new { message = "User's fields are incorrect!" });

            return Ok(response);
        }

        [HttpPost("refreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.Fingerprint))
                return BadRequest(new { message = "Invalid token or fingerprint!" });

            var response = await _userService.RefreshToken(request);

            if (response == null)
                return NotFound(new { message = "Token or fingerprint were not found!" });

            return Ok(response);
        }

        [HttpPost("revokeToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.Fingerprint))
                return BadRequest(new { message = "Token or fingerprint were required!" });

            var response = await _userService.RevokeToken(request);

            if (!response)
                return NotFound(new { message = "Token or fingerprint was not found!" });

            return Ok(new { message = "Token revoked successefuly!" });
        }
    }
}
