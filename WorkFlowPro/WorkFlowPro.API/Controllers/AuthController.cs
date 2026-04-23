using Microsoft.AspNetCore.Mvc;
using WorkFlowPro.Application.Common.Interfaces;
using WorkFlowPro.Application.Common.Models;
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterDto dto)
        {
            // FluentValidation runs automatically before
            // this method body executes
            // If validation fails → 400 returned immediately
            // We never reach this code

            var result = await _authService
                .RegisterAsync(dto);

            // Consistent response format
            return StatusCode(201,
                ApiResponse<AuthResponseDto>
                    .SuccessResult(
                        result,
                        "Registration successful",
                        201));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto dto)
        {
            var result = await _authService
                .LoginAsync(dto);

            return Ok(
                ApiResponse<AuthResponseDto>
                    .SuccessResult(
                        result,
                        "Login successful"));
        }
    }
}