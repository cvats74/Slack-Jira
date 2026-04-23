using Microsoft.EntityFrameworkCore;
using WorkFlowPro.Application.Common.Interfaces;
using WorkFlowPro.Domain.Entities;
using WorkFlowPro.Infrastructure.Data;

namespace WorkFlowPro.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            // Simple fetch by ID
            // Global filter auto-adds IsDeleted = 0
            return await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project?> GetByIdWithDetailsAsync(
            Guid id)
        {
            // Include loads related data in same query
            // SQL: JOIN instead of separate queries
            return await _context.Projects
                .Include(p => p.Owner)
                // Load members with their user info
                .Include(p => p.Members)
                    .ThenInclude(m => m.User)
                // Load tasks for count
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Project>>
            GetByOrganizationAsync(Guid organizationId)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Members)
                .Include(p => p.Tasks)
                .Where(p =>
                    p.OrganizationId == organizationId)
                // Newest projects first
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>>
            GetByUserAsync(Guid userId)
        {
            // Get projects where user is a member
            // OR user is the owner
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Members)
                .Include(p => p.Tasks)
                .Where(p =>
                    p.OwnerId == userId ||
                    p.Members.Any(m =>
                        m.UserId == userId &&
                        m.IsActive))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> IsUserMemberAsync(
            Guid projectId,
            Guid userId)
        {
            return await _context.ProjectMembers
                .AnyAsync(pm =>
                    pm.ProjectId == projectId &&
                    pm.UserId == userId &&
                    pm.IsActive);
        }

        public async Task<bool>
            NameExistsInOrganizationAsync(
                string name,
                Guid organizationId)
        {
            return await _context.Projects
                .AnyAsync(p =>
                    p.Name.ToLower() == name.ToLower() &&
                    p.OrganizationId == organizationId);
        }

        public async Task<Project> CreateAsync(
            Project project)
        {
            await _context.Projects.AddAsync(project);
            return project;
        }

        public  Task<Project> UpdateAsync(
            Project project)
        {
            _context.Projects.Update(project);
            return Task.FromResult(project);
        }

        public async Task AddMemberAsync(
            ProjectMember member)
        {
            await _context.ProjectMembers.AddAsync(member);
        }

        public async Task RemoveMemberAsync(
            Guid projectId,
            Guid userId)
        {
            var member = await _context.ProjectMembers
                .FirstOrDefaultAsync(pm =>
                    pm.ProjectId == projectId &&
                    pm.UserId == userId);

            if (member != null)
            {
                // Soft delete member record
                member.IsActive = false;
                _context.ProjectMembers.Update(member);
            }
        }

        public async Task<ProjectMember?> GetMemberAsync(
            Guid projectId,
            Guid userId)
        {
            return await _context.ProjectMembers
                .Include(pm => pm.User)
                .FirstOrDefaultAsync(pm =>
                    pm.ProjectId == projectId &&
                    pm.UserId == userId &&
                    pm.IsActive);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}