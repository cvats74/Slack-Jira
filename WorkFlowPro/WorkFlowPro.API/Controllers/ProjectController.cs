using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkFlowPro.Application.Common.Interfaces;
using WorkFlowPro.Application.Common.Models;
using WorkFlowPro.Application.Features.Projects.DTOs;

namespace WorkFlowPro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ALL endpoints require JWT token
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(
            IProjectService projectService)
        {
            _projectService = projectService;
        }

        // Helper: Extract current user ID from JWT token
        // Claims were set during token generation
        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            return Guid.Parse(userIdClaim!);
        }

        // Helper: Extract organization ID from JWT token
        private Guid GetOrganizationId()
        {
            var orgIdClaim = User
                .FindFirst("organizationId")?.Value;
            return Guid.Parse(orgIdClaim!);
        }

        // POST /api/project
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateProjectDto dto)
        {
            var userId = GetCurrentUserId();
            var orgId = GetOrganizationId();

            var result = await _projectService
                .CreateAsync(dto, userId, orgId);

            return StatusCode(201,
                ApiResponse<ProjectResponseDto>
                    .SuccessResult(
                        result,
                        "Project created successfully",
                        201));
        }

        // GET /api/project
        [HttpGet]
        public async Task<IActionResult> GetMyProjects()
        {
            var userId = GetCurrentUserId();
            var orgId = GetOrganizationId();

            var result = await _projectService
                .GetMyProjectsAsync(userId, orgId);

            return Ok(
                ApiResponse<IEnumerable<ProjectSummaryDto>>
                    .SuccessResult(result));
        }

        // GET /api/project/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = GetCurrentUserId();

            var result = await _projectService
                .GetByIdAsync(id, userId);

            return Ok(
                ApiResponse<ProjectResponseDto>
                    .SuccessResult(result!));
        }

        // PUT /api/project/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateProjectDto dto)
        {
            var userId = GetCurrentUserId();

            var result = await _projectService
                .UpdateAsync(id, dto, userId);

            return Ok(
                ApiResponse<ProjectResponseDto>
                    .SuccessResult(
                        result,
                        "Project updated successfully"));
        }

        // DELETE /api/project/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetCurrentUserId();

            await _projectService.DeleteAsync(id, userId);

            return Ok(ApiResponse.SuccessResult(
                "Project deleted successfully"));
        }

        // POST /api/project/{id}/members
        [HttpPost("{id}/members")]
        public async Task<IActionResult> AddMember(
            Guid id,
            [FromBody] AddProjectMemberDto dto)
        {
            var userId = GetCurrentUserId();

            var result = await _projectService
                .AddMemberAsync(id, dto, userId);

            return StatusCode(201,
                ApiResponse<ProjectMemberDto>
                    .SuccessResult(
                        result,
                        "Member added successfully",
                        201));
        }

        // DELETE /api/project/{id}/members/{userId}
        [HttpDelete("{id}/members/{memberId}")]
        public async Task<IActionResult> RemoveMember(
            Guid id,
            Guid memberId)
        {
            var currentUserId = GetCurrentUserId();

            await _projectService
                .RemoveMemberAsync(
                    id,
                    memberId,
                    currentUserId);

            return Ok(ApiResponse.SuccessResult(
                "Member removed successfully"));
        }

        // GET /api/project/{id}/members
        [HttpGet("{id}/members")]
        public async Task<IActionResult> GetMembers(Guid id)
        {
            var userId = GetCurrentUserId();

            var result = await _projectService
                .GetMembersAsync(id, userId);

            return Ok(
                ApiResponse<IEnumerable<ProjectMemberDto>>
                    .SuccessResult(result));
        }
    }
}