using Microsoft.AspNetCore.Mvc;
using WorkFlowPro.Application.Common.Interfaces;
using WorkFlowPro.Application.Features.Auth.DTOs;

namespace WorkFlowPro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {

            _authService = authService;
        }

        //post /pi/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {

            try
            {
                var result = await _authService.LoginAsync(dto);

                return Ok(result);
            }

            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }

            catch (Exception ex)
            {
                {
                    return StatusCode(500, new
                    {
                        message = "An error occured.",
                        detail = ex.Message
                    });
                }
            }

        }
    }
}
