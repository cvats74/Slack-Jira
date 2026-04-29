using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Entities;

namespace WorkFlowPro.Application.Common.Interfaces
{
    public interface IWorkItemRepository 
    {
        Task<WorkItem?> GetByIdAsync(Guid id);

        Task<WorkItem?> GetByIdWithDetailsAsync(Guid id);

        Task<IEnumerable<WorkItem>> GetByProjectAsync(Guid projectId);

        Task<IEnumerable<WorkItem>> GetByProjectAndStatusAsync(Guid projectId, Domain.Enums.TaskStatus status);

        Task<IEnumerable<WorkItem>> GetByAssigneeAsync(Guid userId);

        Task<IEnumerable<WorkItem>> GetSubTaskAsync(Guid parentTaskId);

        Task<bool> BelongsToProjectAsync(Guid workItemId,Guid projectId);

        Task<WorkItem> CreateAsync(WorkItem workItem);

        Task<WorkItem> UpdateAsync(WorkItem workItem);

        Task<int> SaveChangesAsync();
    }
}
