using WorkFlowPro.Application.Features.Projects.DTOs;
using WorkFlowPro.Domain.Entities;

namespace WorkFlowPro.Application.Common.Interfaces
{
    public interface IProjectRepository
    {
        // Get single project by ID
        // includes members and tasks count
        Task<Project?> GetByIdAsync(Guid id);

        // Get project with full details
        // (members, tasks, owner info)
        Task<Project?> GetByIdWithDetailsAsync(Guid id);

        // Get all projects for an organization
        Task<IEnumerable<Project>> GetByOrganizationAsync(
            Guid organizationId);

        // Get all projects a specific user is member of
        Task<IEnumerable<Project>> GetByUserAsync(
            Guid userId);

        // Check if user is member of project
        Task<bool> IsUserMemberAsync(
            Guid projectId,
            Guid userId);

        // Check if project name exists in organization
        Task<bool> NameExistsInOrganizationAsync(
            string name,
            Guid organizationId);

        // Create new project
        Task<Project> CreateAsync(Project project);

        // Update existing project
        Task<Project> UpdateAsync(Project project);

        // Add member to project
        Task AddMemberAsync(ProjectMember member);

        // Remove member from project
        Task RemoveMemberAsync(
            Guid projectId,
            Guid userId);

        // Get project member record
        Task<ProjectMember?> GetMemberAsync(
            Guid projectId,
            Guid userId);

        // Save all pending changes
        Task<int> SaveChangesAsync();
    }
}