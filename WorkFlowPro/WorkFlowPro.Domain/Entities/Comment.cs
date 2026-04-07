using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowPro.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public bool IsEdited { get; set; } = false;

        public Guid WorkItemId { get; set; }
        public WorkItem? WorkItem { get; set; }

        public Guid AuthorId { get; set; }
        public User? Author { get; set; }s

    }
}
