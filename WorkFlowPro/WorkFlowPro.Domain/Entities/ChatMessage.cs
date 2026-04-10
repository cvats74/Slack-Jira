using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Common;


namespace WorkFlowPro.Domain.Entities
{
    public class ChatMessage : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public bool IsEdited { get; set; } = false;
        public DateTime? EditedAt { get; set; }

        public Guid ChannelId { get; set; }
        public ChatChannel? Channel { get; set; }

        public Guid SenderId { get; set; }
        public User? Sender { get; set; }
    }
}
