using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Common;


namespace WorkFlowPro.Domain.Entities
{
    public class Label : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ColorHex { get; set; } = "#6B7280";

        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        public ICollection<WorkItem> WorkItems { get; set; }
            = new List<WorkItem>();
    }
}
