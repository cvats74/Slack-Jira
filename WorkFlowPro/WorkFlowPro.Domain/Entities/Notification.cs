using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Common;

namespace WorkFlowPro.Domain.Entities
{
    public enum NotificationType
    {
        TaskAssigned = 1,
        TaskUpdated = 2,
        CommentAdded = 3,
        MentionedInComment = 4,
        ProjectInvitation = 5,
        DeadlineApproaching = 6
    }

    public class Notification : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }
        public string? ActionUrl { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
