using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Application.Features.Auth.DTOs;

namespace WorkFlowPro.Application.Common.Interfaces
{
    public interface IAuthService
    {

        // Register new user + create their organization
        // Returns auth response with JWT token
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

        //Login exiting user
        Task<AuthResponseDto> LoginAsync(LoginDto dto);

        // Register new user + create their organization
        // Returns auth response with JWT token
        string GenerateJwtToken(Domain.Entities.User user);

    }
}
