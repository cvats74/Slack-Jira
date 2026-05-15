using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Enums;

namespace WorkFlowPro.Application.Features.WorkItems.DTOs
{
    public class CreateWorkItemDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength (500)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(5000)]
        public string Description { get; set; } = string.Empty ;

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public DateTime? DueDate { get; set; }
        public Guid? AssigneeId { get; set; }

        public Guid? ParentTaskId { get; set; }

        public int? EstimatedHours { get; set; }

    }
    //update whole project
    public class UpdateWorkItemDto
    {
        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(5000)]
        public string Description { get; set; } = string.Empty;

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public DateTime? DueDate { get; set; }
        public Guid? AssigneeId { get; set; }

        public Guid? ParentTaskId { get; set; }

        public int? EstimatedHours { get; set; }

        public int? ActualHours { get; set; }

    }
    //update only status
    public class UpdateWorkItemStatusDto
    {
        [Required]
        public Domain.Enums.TaskStatus Status {  get; set; }
    }

    public class WorkItemResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
            = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? EstimatedHours { get; set; }
        public int? ActualHours { get; set; }

        // Project this task belongs to
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
            = string.Empty;

        // Who created this task
        public Guid ReporterId { get; set; }
        public string ReporterName { get; set; }
            = string.Empty;

        // Who is doing this task
        public Guid? AssigneeId { get; set; }
        public string? AssigneeName { get; set; }

        // Subtasks if any
        public List<WorkItemSummaryDto> SubTasks { get; set; }
            = new();

        // Comments count
        public int CommentCount { get; set; }
    }

    public class WorkItemSummaryDto 
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public Guid? AssigneeId { get; set; }
        public string? AssigneeName { get; set; }
        public int SubTaskCount { get; set; }
        public int CommentCount { get; set; }

        //overdue tasks
        public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.UtcNow && Status != "Done" && Status != "Completed";
    }

}
