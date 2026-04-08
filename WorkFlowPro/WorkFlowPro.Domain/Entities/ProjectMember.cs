using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Common;
using WorkFlowPro.Domain.Enums;

namespace WorkFlowPro.Domain.Entities
{
    public class ProjectMember : BaseEntity
    {
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public UserRole Role { get; set; } = UserRole.Employee;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
