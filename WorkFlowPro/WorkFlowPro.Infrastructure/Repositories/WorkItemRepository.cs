using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Application.Common.Interfaces;
using WorkFlowPro.Domain.Entities;
using WorkFlowPro.Infrastructure.Data;

namespace WorkFlowPro.Infrastructure.Repositories
{
    public class WorkItemRepository : IWorkItemRepository
    {

        private readonly AppDbContext _context;
        public WorkItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> BelongsToProjectAsync(Guid workItemId, Guid projectId)
        {
            return await _context.WorkItems.AnyAsync(w => w.Id  == workItemId && w.ProjectId == projectId); 
        }

        public async Task<WorkItem> CreateAsync(WorkItem workItem)
        {
            await _context.WorkItems.AddAsync(workItem);
            return workItem;
        }

        public async Task<IEnumerable<WorkItem>> GetByAssigneeAsync(Guid userId)
        {
            return await _context.WorkItems
                .Include(w => w.Project)
                .Where(w => w.AssigneeId == userId)
                .OrderByDescending(w => w.CreatedAt).ToListAsync();
        }

        public async Task<WorkItem?> GetByIdAsync(Guid id)
        {
            return await _context.WorkItems.FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<WorkItem?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.WorkItems.Include(w => w.Project)
                .Include(w => w.Assignee)
                .Include(w => w.Reporter)
                .Include(w => w.SubTasks)
                .Include(w => w.Comments)
                .FirstOrDefaultAsync( w  => w.Id == id);
        }

        public async Task<IEnumerable<WorkItem>> GetByProjectAndStatusAsync(Guid projectId, Domain.Enums.TaskStatus status)
        {
            return await _context.WorkItems
                .Include(w => w.Assignee)
                .Include(w => w.SubTasks)
                .Where(w =>
                    w.ProjectId == projectId &&
                    w.Status == status &&
                    w.ParentTaskId == null)
                .OrderBy(w => w.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkItem>> GetByProjectAsync(Guid projectId)
        {
            return await _context.WorkItems
                .Include(w => w.Assignee)
                .Include(w => w.SubTasks)
                .Where(w =>
                    w.ProjectId == projectId &&
                    w.ParentTaskId == null)
                .OrderBy(w => w.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkItem>> GetSubTaskAsync(Guid parentTaskId)
        {
            return await _context.WorkItems
                .Include(w => w.Assignee)
                .Where(w => w.ParentTaskId == parentTaskId)
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task<WorkItem> UpdateAsync(WorkItem workItem)
        {
            _context.Update(workItem);
            return Task.FromResult(workItem);
        }
    }
}
