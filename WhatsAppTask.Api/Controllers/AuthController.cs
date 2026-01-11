using Microsoft.AspNetCore.Mvc;
using WhatsAppTask.BLL.Interfaces;
using WhatsAppTask.DTO;

namespace WhatsAppTask.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto request)
        {
            var user = _userService.Login(request.UsernameOrEmail, request.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.Role
            });
        }
        [HttpPost("register")]
        public IActionResult Register(CreateUserRequestDto request)
        {
            try
            {
                var user = _userService.CreateUser(
                    request.Username,
                    request.Email,
                    request.Password,
                    request.Role
                );

                return Ok(new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.Role
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
