using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Enums;

namespace WorkFlowPro.Application.Features.Projects.DTOs
{
    public class CreateProjectDTO
    {
        [Required(ErrorMessage ="Project Name is Required")]
        [MaxLength(200, ErrorMessage = "Name cannot extent 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Project Description is Required")]
        [MaxLength(2000, ErrorMessage = "Description cannot extent 2000 characters")]
        public string Description { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        public string? CoverImageUrl { get; set; }

    }

    public class UpdateProjectDTO
    {
        [Required(ErrorMessage = "Project Name is Required")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Project Description is Required")]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        public string? CoverImageUrl { get; set; }

        public  ProjectStatus Status { get; set; }
    }

    public class ProjectResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? CoverImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // Who created it
        public string OwnerName { get; set; } = string.Empty;

        // How many members
        public int MemberCount { get; set; }

        // How many tasks total
        public int TaskCount { get; set; }

        // How many tasks completed
        public int CompletedTaskCount { get; set; }

    }

    public class ProjectSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public int MemberCount { get; set; }
        public int TaskCount { get; set; }

        // Progress percentage 0-100
        public int ProgressPercentage { get; set; }

    }
    public class AddProjectMemberDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public Guid UserId { get; set; }

        // What role this member has in project
        public UserRole Role { get; set; }
            = UserRole.Employee;
    }

    // Member info in response
    public class ProjectMemberDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
    }
}
