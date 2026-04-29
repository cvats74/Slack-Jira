using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Application.Common.Interfaces;
using WorkFlowPro.Application.Features.WorkItems.DTOs;
using WorkFlowPro.Domain.Entities;

namespace WorkFlowPro.Infrastructure.Services
{
    public class WorkItemService : IWorkItemService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkItemRepository _workItemRepository;
        private readonly IUserRepository _userRepository;

        public WorkItemService(IProjectRepository projectRepository, IWorkItemRepository workItemRepository, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _workItemRepository = workItemRepository;
            _userRepository = userRepository;
        }

        public async Task<WorkItemResponseDto> CreateAsync(Guid projectId, CreateWorkItemDto dto, Guid currentUserId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);

            if (project == null) {

                throw new KeyNotFoundException("Project not found");
            }

            var isMember = await _projectRepository.IsUserMemberAsync(projectId, currentUserId);

            if(!isMember && project.OwnerId != currentUserId)
            {
                throw new UnauthorizedAccessException("Only project members can create tasks");
            }

            if (dto.AssigneeId.HasValue)
            {
                var assigneeIsMember = await _projectRepository.IsUserMemberAsync(projectId, currentUserId);
                if (!assigneeIsMember)
                {
                    throw new InvalidOperationException("Assignee must be project member");

                }
            }
            var workItem = new WorkItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                AssigneeId = dto.AssigneeId,
                ParentTaskId = dto.ParentTaskId,
                EstimatedHours = dto.EstimatedHours,
                ProjectId = projectId,
                ReporterId = currentUserId,
                Status = Domain.Enums.TaskStatus.ToDo,
                TenantId = project.TenantId,
                CreatedBy = currentUserId
            };

            await _workItemRepository.CreateAsync(workItem);
            await _workItemRepository.SaveChangesAsync();

            var created = await _workItemRepository.GetByIdWithDetailsAsync(workItem.Id);
            return MapToResponseDto(created!);
        }

       

        public async Task DeleteAsync(Guid workItemId, Guid currentUserId)
        {
            var workItem = await _workItemRepository.GetByIdWithDetailsAsync(currentUserId);
            if (workItem != null)
            {
                throw new KeyNotFoundException("Task not found");
            }

            var isMember = await _projectRepository.IsUserMemberAsync(workItem.ProjectId, currentUserId);
            var isOwner = workItem.Project?.OwnerId == currentUserId;

            if (!isMember && !isOwner)
            {
                throw new UnauthorizedAccessException("Access Denied");
            }
        }

        public Task<WorkItemResponseDto> GetByIdAsync(Guid workItemId, Guid currentUserId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkItemSummaryDto>> GetByProjectAsync(Guid projectId, Guid currentUserId)
        {
            throw new NotImplementedException();
        }

        public Task<WorkItemResponseDto> UpdateAsync(Guid workItemId, UpdateWorkItemDto dto, Guid currentUserId)
        {
            throw new NotImplementedException();
        }

        public Task<WorkItemResponseDto> UpdateStatusAsync(Guid workItemId, UpdateWorkItemStatusDto dto, Guid currentUserId)
        {
            throw new NotImplementedException();
        }

        private static WorkItemResponseDto MapToResponseDto(WorkItem w)
        {
            return new WorkItemResponseDto
            {
                Id = w.Id,
                Title = w.Title,
                Description = w.Description,
                Status = w.Status.ToString(),
                Priority = w.Priority.ToString(),
                DueDate = w.DueDate,
                CreatedAt = w.CreatedAt,
                EstimatedHours = w.EstimatedHours,
                ActualHours = w.ActualHours,
                ProjectId = w.ProjectId,
                ProjectName = w.Project?.Name
                    ?? string.Empty,
                ReporterId = w.ReporterId,
                ReporterName = w.Reporter?.FullName
                    ?? string.Empty,
                AssigneeId = w.AssigneeId,
                AssigneeName = w.Assignee?.FullName,
                SubTasks = w.SubTasks?
                    .Select(s => MapToSummaryDto(s))
                    .ToList() ?? new(),
                CommentCount = w.Comments?.Count ?? 0
            };
        }

        private static WorkItemSummaryDto MapToSummaryDto(
            WorkItem w)
        {
            return new WorkItemSummaryDto
            {
                Id = w.Id,
                Title = w.Title,
                Status = w.Status.ToString(),
                Priority = w.Priority.ToString(),
                DueDate = w.DueDate,
                AssigneeId = w.AssigneeId,
                AssigneeName = w.Assignee?.FullName,
                SubTaskCount = w.SubTasks?.Count ?? 0,
                CommentCount = w.Comments?.Count ?? 0
            };
        }
    }
}
