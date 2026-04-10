using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Common;
using WorkFlowPro.Domain.Enums;

namespace WorkFlowPro.Domain.Entities
{
    public  class User : BaseEntity
    {
        //Identity
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        //Role status
        public  UserRole Role { get; set; } = UserRole.Employee;
        public UserStatus Status { get; set; } = UserStatus.Pending;

        //profile

        public string? ProfilePicture {  get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? LastloginAt{ get; set; }

        //organisation

        public Guid OrganizationId { get; set; }
        public Organization? Organization { get; set; }

        //Navigation 
        public ICollection<ProjectMember> ProjectMemberships { get; set; }
            = new List<ProjectMember>();
        public ICollection<Notification> Notifications { get; set; }
            = new List<Notification>();

        // Computed - no DB column
        public string FullName => $"{FirstName} {LastName}";
    }
}
