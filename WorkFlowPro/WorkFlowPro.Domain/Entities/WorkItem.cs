using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Common;

namespace WorkFlowPro.Domain.Entities
{
    public class WorkItem : BaseEntity
    {
        public WorkItem()
        {
            Comments = new List<Comment>();

            Attachments = new List<FileAttachment>();

            Labels = new List<Label>();

            SubTasks = new List<WorkItem>();
        }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; } = TaskStatus.Todo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateTime? DueDate { get; set; }
        public int? EstimatedHours { get; set; }
        public int? ActualHours { get; set; }

        // Project relationship
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        // Assignee
        public Guid? AssigneeId { get; set; }
        public User? Assignee { get; set; }

        // Reporter
        public Guid ReporterId { get; set; }
        public User? Reporter { get; set; }

        // Self-referencing for SubTasks
        public Guid? ParentTaskId { get; set; }
        public WorkItem? ParentTask { get; set; }
        public ICollection<WorkItem> SubTasks { get; set; }

        // Navigation
        public ICollection<Comment> Comments { get; set; }
        public ICollection<FileAttachment> Attachments { get; set; }
        public ICollection<Label> Labels { get; set; }
    }
}
