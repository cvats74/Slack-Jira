using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Application.Common.Interfaces;
using WorkFlowPro.Application.Features.Auth.DTOs;
using WorkFlowPro.Domain.Entities;
using WorkFlowPro.Domain.Enums;

namespace WorkFlowPro.Infrastructure.Services
{
    public class AuthService : IAuthService
    {

        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _orgRepository;
        private readonly IJwtService _jwtService;

        // Constructor injection — DI provides all dependencies
        public AuthService(IUserRepository userRepository,
            IOrganizationRepository orgRepository,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _orgRepository = orgRepository;
            _jwtService = jwtService;
        }
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // STEP 1: Check email not already registered
            var emailExists = await _userRepository.EmailExistsAsync(dto.Email);

            if (emailExists)
            {
                throw new UnauthorizedAccessException("Email already Registered");
            }

            //create organisation first
            //first user = admin
            var slug = GenerateSlug(dto.OrganizationName);
            var organisaton = new Organization
            {
                Name = dto.OrganizationName,
                Slug = slug,
                IsActive = true
            };
            var createdOrg = await _orgRepository.CreateAsync(organisaton);

            //hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            //create User
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = UserRole.Admin,
                Status = UserStatus.Active,
                OrganizationId = createdOrg.Id,
                TenantId = createdOrg.TenantId,
            };

            var createdUser = await _userRepository.CreateAsync(user);

            // STEP 5: Save both org and user in one transaction
            await _userRepository.SaveChangesAsync();

            // STEP 6: Update IDs after save
            createdUser.OrganizationId = createdOrg.Id;

            // STEP 7: Generate JWT token
            var token = _jwtService.GenerateToken(createdUser);

            // STEP 8: Return response
            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = _jwtService.GenerateRefreshToken(),
                Email = createdUser.FullName,
                Role = createdUser.Role.ToString(),
                TokenExpiry = _jwtService.GetTokenExpiry()
            };
        }
        public string GenerateJwtToken(User user)
        {
            return _jwtService.GenerateToken(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // STEP 1: Find user by email
            var user = await _userRepository
                .GetByEmailAsync(dto.Email);

            // STEP 2: Check user exists
            if (user == null)
            {
                // Same error for wrong email AND wrong password
                // Security: don't tell attacker which was wrong
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            // STEP 3: Check account is active
            if (user.Status != UserStatus.Active)
            {
                throw new UnauthorizedAccessException(
                    "Account is not active.");
            }

            // STEP 4: Verify password
            var passwordValid = BCrypt.Net.BCrypt
                .Verify(dto.Password, user.PasswordHash);

            if (!passwordValid)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            // STEP 5: Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            // STEP 6: Generate and return token
            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = _jwtService.GenerateRefreshToken(),
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                TokenExpiry = _jwtService.GetTokenExpiry()
            };
        }

        private static string GenerateSlug(string name) {

            return name.ToLower().Trim()
                .Replace(" ", "-")
                .Replace("_", " ");
        
        }
        
    }
}
