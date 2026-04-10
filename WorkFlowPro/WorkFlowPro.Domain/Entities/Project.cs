using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Common;
using WorkFlowPro.Domain.Enums;

namespace WorkFlowPro.Domain.Entities
{
    public class Project :BaseEntity
    {
        public Project()
        {
            Members = new List<ProjectMember>();
            Tasks = new List<WorkItem>();
            ChatChannels = new List<ChatChannel>();


        }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Planning;
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        // Organization relationship
        public Guid OrganizationId { get; set; }
        public Organization? Organization { get; set; }

        //Project Owner

        public Guid OwnerId { get; set; }
        public User? Owner { get; set; }

        //Navigation
        public ICollection<ProjectMember> Members { get; set; }
        public ICollection<WorkItem> Tasks { get; set; }
        public ICollection<ChatChannel> ChatChannels { get; set; }
    }
}
