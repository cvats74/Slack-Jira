using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Application.Common.Interfaces;
using WorkFlowPro.Application.Features.Projects.DTOs;
using WorkFlowPro.Domain.Entities;
using WorkFlowPro.Infrastructure.Data;

namespace WorkFlowPro.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        public ProjectRepository(AppDbContext context) {
        
            _context = context;
        }

        public async Task AddMemberAsync(ProjectMember member)
        {
            await _context.ProjectMembers.AddAsync(member);
        }

        public async Task<Project> CreateAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            return project;
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project?> GetByIdWithDetailsAsync(Guid id)
        {
            //sql joins 
            return await _context.Projects.Include(p => p.Owner)
                .Include(p => p.Members)
                .ThenInclude(m => m.User)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Project>> GetByOrganizationAsync(Guid organisationId)
        {
            return await _context.Projects.Include(p =>p.Owner)
                .Include(p =>p.Members)
                .Include(p => p.Tasks)
                .Where(p => p.OrganizationId == organisationId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetByUserAsync(Guid userId)
        {
           return await _context.Projects.Include(p => p.Owner)
                .Include(p => p.Members)
                .Include(p => p.Tasks)
                .Where(p => p.OwnerId == userId || p.Members.Any(m => m.UserId == userId && m.IsActive))
                .OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public Task<Project?> GetMemberAsync(Guid projectId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsUserMemberAsync(Guid projectId, Guid userId)
        {
            return await _context.ProjectMembers.AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId && pm.IsActive);

        }

        public async Task<bool> NameExistsInOrganizationAsync(string name, Guid organizationId)
        {
            return await _context.Projects.AnyAsync(p => p.Name == name && p.OrganizationId == organizationId);
        }

        public Task RemoveMemberAsync(Guid projectId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<Project> UpdateAsync(Project project)
        {
           _context.Projects.Update(project);
            return Task.FromResult(project);
        }

        Task IProjectRepository.AddMemberAsync(ProjectMember member)
        {
            return AddMemberAsync(member);
        }
    }
}
