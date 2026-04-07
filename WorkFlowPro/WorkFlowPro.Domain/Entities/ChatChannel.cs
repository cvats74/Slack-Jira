using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowPro.Domain.Entities
{
    public class ChatChannel : BaseEntity
    {
        public ChatChannel()
        {
            Messages = new List<ChatMessage>();
        }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsDefault { get; set; } = false;

        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        public ICollection<ChatMessage> Messages { get; set; }
    }
}
