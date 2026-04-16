using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Application.Features.Projects.DTOs;

namespace WorkFlowPro.Application.Common.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectResponseDto> CreateAsync(CreateProjectDTO dto, Guid currenUserId, Guid organisationId);


        Task<ProjectResponseDto> GetByIdAsync(Guid projectId, Guid currentUserId);

        Task<IEnumerable<ProjectSummaryDto>> GetMyProjectsAsync(Guid currentUserId, Guid organisationId);

        Task<ProjectResponseDto> UpdateAsync(Guid projectId, UpdateProjectDTO dto, Guid currentUserId);

        Task DeleteAsync(Guid projectId, Guid currentUserId);

        Task<ProjectMemberDto> AddMemberAsync(Guid projectId, Guid memberUserId, Guid currentUserId);

        Task<IEnumerable<ProjectMemberDto>> GetMembersAsync(Guid projectId, Guid currentUserId);
    }

}
