using WorkFlowPro.Application.Common.Interfaces;
using WorkFlowPro.Application.Features.Projects.DTOs;
using WorkFlowPro.Domain.Entities;
using WorkFlowPro.Domain.Enums;

namespace WorkFlowPro.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public ProjectService(
            IProjectRepository projectRepository,
            IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task<ProjectResponseDto> CreateAsync(
            CreateProjectDto dto,
            Guid currentUserId,
            Guid organizationId)
        {
            // STEP 1: Validate no duplicate name
            var nameExists = await _projectRepository
                .NameExistsInOrganizationAsync(
                    dto.Name,
                    organizationId);

            if (nameExists)
                throw new InvalidOperationException(
                    $"Project '{dto.Name}' already exists.");

            // STEP 2: Create project entity
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                DueDate = dto.DueDate,
                CoverImageUrl = dto.CoverImageUrl,
                Status = ProjectStatus.Planning,
                OwnerId = currentUserId,
                OrganizationId = organizationId,
                TenantId = organizationId,
                CreatedBy = currentUserId
            };

            var created = await _projectRepository
                .CreateAsync(project);

            // STEP 3: Auto-add creator as Admin member
            // Creator should always be a member of their project
            var creatorMember = new ProjectMember
            {
                ProjectId = created.Id,
                UserId = currentUserId,
                Role = UserRole.Admin,
                JoinedAt = DateTime.UtcNow,
                IsActive = true,
                TenantId = organizationId,
                CreatedBy = currentUserId
            };

            await _projectRepository
                .AddMemberAsync(creatorMember);

            // STEP 4: Save both project and membership
            await _projectRepository.SaveChangesAsync();

            // STEP 5: Get owner info for response
            var owner = await _userRepository
                .GetByIdAsync(currentUserId);

            // STEP 6: Map to DTO and return
            return MapToResponseDto(created, owner);
        }

        public async Task<ProjectResponseDto?> GetByIdAsync(
            Guid projectId,
            Guid currentUserId)
        {
            // Load project with all related data
            var project = await _projectRepository
                .GetByIdWithDetailsAsync(projectId);

            if (project == null)
                throw new KeyNotFoundException(
                    "Project not found.");

            // Verify user has access to this project
            var hasAccess =
                project.OwnerId == currentUserId ||
                await _projectRepository
                    .IsUserMemberAsync(
                        projectId,
                        currentUserId);

            if (!hasAccess)
                throw new UnauthorizedAccessException(
                    "You don't have access to this project.");

            return MapToResponseDto(
                project,
                project.Owner);
        }

        public async Task<IEnumerable<ProjectSummaryDto>>
            GetMyProjectsAsync(
                Guid currentUserId,
                Guid organizationId)
        {
            var projects = await _projectRepository
                .GetByUserAsync(currentUserId);

            // Map each project to summary DTO
            return projects.Select(p =>
                MapToSummaryDto(p)).ToList();
        }

        public async Task<ProjectResponseDto> UpdateAsync(
            Guid projectId,
            UpdateProjectDto dto,
            Guid currentUserId)
        {
            var project = await _projectRepository
                .GetByIdAsync(projectId);

            if (project == null)
                throw new KeyNotFoundException(
                    "Project not found.");

            // Only owner or admin member can update
            if (project.OwnerId != currentUserId)
            {
                var member = await _projectRepository
                    .GetMemberAsync(
                        projectId,
                        currentUserId);

                if (member == null ||
                    member.Role != UserRole.Admin)
                    throw new UnauthorizedAccessException(
                        "Only project owner or admin can update.");
            }

            // Update fields
            project.Name = dto.Name;
            project.Description = dto.Description;
            project.StartDate = dto.StartDate;
            project.DueDate = dto.DueDate;
            project.CoverImageUrl = dto.CoverImageUrl;
            project.Status = dto.Status;
            project.UpdatedBy = currentUserId;

            await _projectRepository.UpdateAsync(project);
            await _projectRepository.SaveChangesAsync();

            var owner = await _userRepository
                .GetByIdAsync(project.OwnerId);

            return MapToResponseDto(project, owner);
        }

        public async Task DeleteAsync(
            Guid projectId,
            Guid currentUserId)
        {
            var project = await _projectRepository
                .GetByIdAsync(projectId);

            if (project == null)
                throw new KeyNotFoundException(
                    "Project not found.");

            // Only PROJECT OWNER can delete
            // Not even admins — only the creator
            if (project.OwnerId != currentUserId)
                throw new UnauthorizedAccessException(
                    "Only project owner can delete.");

            // Soft delete — set IsDeleted flag
            project.IsDeleted = true;
            project.DeletedAt = DateTime.UtcNow;
            project.DeletedBy = currentUserId;

            await _projectRepository.UpdateAsync(project);
            await _projectRepository.SaveChangesAsync();
        }

        public async Task<ProjectMemberDto> AddMemberAsync(
            Guid projectId,
            AddProjectMemberDto dto,
            Guid currentUserId)
        {
            // Verify project exists
            var project = await _projectRepository
                .GetByIdAsync(projectId);

            if (project == null)
                throw new KeyNotFoundException(
                    "Project not found.");

            // Only owner or admin can add members
            if (project.OwnerId != currentUserId)
            {
                var requestingMember =
                    await _projectRepository
                        .GetMemberAsync(
                            projectId,
                            currentUserId);

                if (requestingMember == null ||
                    requestingMember.Role != UserRole.Admin)
                    throw new UnauthorizedAccessException(
                        "Only admin can add members.");
            }

            // Verify user to add exists
            var userToAdd = await _userRepository
                .GetByIdAsync(dto.UserId);

            if (userToAdd == null)
                throw new KeyNotFoundException(
                    "User not found.");

            // Check not already a member
            var alreadyMember = await _projectRepository
                .IsUserMemberAsync(projectId, dto.UserId);

            if (alreadyMember)
                throw new InvalidOperationException(
                    "User is already a member.");

            // Create membership
            var member = new ProjectMember
            {
                ProjectId = projectId,
                UserId = dto.UserId,
                Role = dto.Role,
                JoinedAt = DateTime.UtcNow,
                IsActive = true,
                TenantId = project.TenantId,
                CreatedBy = currentUserId
            };

            await _projectRepository.AddMemberAsync(member);
            await _projectRepository.SaveChangesAsync();

            // Return member info
            return new ProjectMemberDto
            {
                UserId = userToAdd.Id,
                FullName = userToAdd.FullName,
                Email = userToAdd.Email,
                Role = dto.Role.ToString(),
                JoinedAt = member.JoinedAt
            };
        }

        public async Task RemoveMemberAsync(
            Guid projectId,
            Guid memberUserId,
            Guid currentUserId)
        {
            var project = await _projectRepository
                .GetByIdAsync(projectId);

            if (project == null)
                throw new KeyNotFoundException(
                    "Project not found.");

            // Cannot remove project owner
            if (memberUserId == project.OwnerId)
                throw new InvalidOperationException(
                    "Cannot remove project owner.");

            // Only owner/admin OR member removing themselves
            bool isSelf = memberUserId == currentUserId;
            bool isOwner = project.OwnerId == currentUserId;

            if (!isSelf && !isOwner)
            {
                var requestingMember =
                    await _projectRepository
                        .GetMemberAsync(
                            projectId,
                            currentUserId);

                if (requestingMember?.Role != UserRole.Admin)
                    throw new UnauthorizedAccessException(
                        "Not authorized to remove members.");
            }

            await _projectRepository
                .RemoveMemberAsync(
                    projectId,
                    memberUserId);

            await _projectRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProjectMemberDto>>
            GetMembersAsync(
                Guid projectId,
                Guid currentUserId)
        {
            var project = await _projectRepository
                .GetByIdWithDetailsAsync(projectId);

            if (project == null)
                throw new KeyNotFoundException(
                    "Project not found.");

            // Verify requester is a member
            var hasAccess =
                project.OwnerId == currentUserId ||
                await _projectRepository
                    .IsUserMemberAsync(
                        projectId,
                        currentUserId);

            if (!hasAccess)
                throw new UnauthorizedAccessException(
                    "Access denied.");

            return project.Members
                .Where(m => m.IsActive)
                .Select(m => new ProjectMemberDto
                {
                    UserId = m.UserId,
                    FullName = m.User?.FullName
                        ?? string.Empty,
                    Email = m.User?.Email
                        ?? string.Empty,
                    Role = m.Role.ToString(),
                    JoinedAt = m.JoinedAt
                })
                .ToList();
        }

        // =============================================
        // PRIVATE MAPPER METHODS
        // =============================================

        private static ProjectResponseDto MapToResponseDto(
            Project project,
            Domain.Entities.User? owner)
        {
            var completedTasks = project.Tasks?
                .Count(t => t.Status ==
                    Domain.Enums.TaskStatus.Done) ?? 0;

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = project.Status.ToString(),
                StartDate = project.StartDate,
                DueDate = project.DueDate,
                CoverImageUrl = project.CoverImageUrl,
                CreatedAt = project.CreatedAt,
                OwnerName = owner?.FullName ?? string.Empty,
                MemberCount = project.Members?.Count ?? 0,
                TaskCount = project.Tasks?.Count ?? 0,
                CompletedTaskCount = completedTasks
            };
        }

        private static ProjectSummaryDto MapToSummaryDto(
            Project project)
        {
            var totalTasks = project.Tasks?.Count ?? 0;
            var completedTasks = project.Tasks?
                .Count(t => t.Status ==
                    Domain.Enums.TaskStatus.Done) ?? 0;

            var progress = totalTasks > 0
                ? (int)Math.Round(
                    (double)completedTasks /
                    totalTasks * 100)
                : 0;

            return new ProjectSummaryDto
            {
                Id = project.Id,
                Name = project.Name,
                Status = project.Status.ToString(),
                DueDate = project.DueDate,
                MemberCount = project.Members?.Count ?? 0,
                TaskCount = totalTasks,
                ProgressPercentage = progress
            };
        }
    }
}