using WorkFlowPro.Application.Features.WorkItems.DTOs;
using WorkFlowPro.Domain.Enums;

namespace WorkFlowPro.Application.Common.Interfaces
{
    public interface IWorkItemService
    {
        // Create task inside a project
        Task<WorkItemResponseDto> CreateAsync(
            Guid projectId,
            CreateWorkItemDto dto,
            Guid currentUserId);

        // Get single task details
        Task<WorkItemResponseDto> GetByIdAsync(
            Guid workItemId,
            Guid currentUserId);

        // Get ALL tasks for a project (for Kanban)
        Task<IEnumerable<WorkItemSummaryDto>> GetByProjectAsync(
                Guid projectId,
                Guid currentUserId);

        // Update task details
        Task<WorkItemResponseDto> UpdateAsync(
            Guid workItemId,
            UpdateWorkItemDto dto,
            Guid currentUserId);

        // Change ONLY the status (drag on Kanban)
        Task<WorkItemResponseDto> UpdateStatusAsync(
            Guid workItemId,
            UpdateWorkItemStatusDto dto,
            Guid currentUserId);

        // Soft delete task
        Task DeleteAsync(
            Guid workItemId,
            Guid currentUserId);
    }
}