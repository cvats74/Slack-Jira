using WorkFlowPro.Application.Features.Projects.DTOs;

namespace WorkFlowPro.Application.Common.Interfaces
{
    public interface IProjectService
    {
        // currentUserId = who is making the request
        // Extracted from JWT token

        Task<ProjectResponseDto> CreateAsync(
            CreateProjectDto dto,
            Guid currentUserId,
            Guid organizationId);

        Task<ProjectResponseDto?> GetByIdAsync(
            Guid projectId,
            Guid currentUserId);

        Task<IEnumerable<ProjectSummaryDto>> GetMyProjectsAsync(
            Guid currentUserId,
            Guid organizationId);

        Task<ProjectResponseDto> UpdateAsync(
            Guid projectId,
            UpdateProjectDto dto,
            Guid currentUserId);

        Task DeleteAsync(
            Guid projectId,
            Guid currentUserId);

        Task<ProjectMemberDto> AddMemberAsync(
            Guid projectId,
            AddProjectMemberDto dto,
            Guid currentUserId);

        Task RemoveMemberAsync(
            Guid projectId,
            Guid memberUserId,
            Guid currentUserId);

        Task<IEnumerable<ProjectMemberDto>> GetMembersAsync(
            Guid projectId,
            Guid currentUserId);
    }
}