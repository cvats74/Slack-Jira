using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Application.Features.Projects.DTOs;
using WorkFlowPro.Domain.Entities;

namespace WorkFlowPro.Application.Common.Interfaces
{
    public  interface IProjectRepository
    {
        
        // Get single project by ID
        // includes members and tasks count
        Task<Project?> GetByIdAsync(Guid id);

        // Get project with full details
        // (members, tasks, owner info)
        Task<Project?> GetByIdWithDetailsAsync(Guid id);

        Task<IEnumerable<Project>> GetByOrganizationAsync(Guid organisationId);

        // Get all projects a specific user is member of
        Task<IEnumerable<Project>> GetByUserAsync(Guid userId);

        //checking if user is member of proj
        Task<bool> IsUserMemberAsync(Guid projectId,Guid userId);

        //check if proj name exists in org
        Task<bool> NameExistsInOrganizationAsync(string name, Guid organizationId);

        //create new proj
        Task<Project> CreateAsync(Project project);

        //update proj
        Task<Project> UpdateAsync(Project project);
        Task AddMemberAsync(ProjectMember member);

        //Remove member from Project
        Task RemoveMemberAsync(Guid projectId, Guid userId);

        //get project member record
        Task<Project?> GetMemberAsync(Guid projectId, Guid userId);

        //save all
        Task<int> SaveChangesAsync();
    }

}
