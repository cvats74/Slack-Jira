using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowPro.Domain.Entities
{
    public class FileAttachment : BaseEntity
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }

        public Guid WorkItemId { get; set; }
        public WorkItem? WorkItem { get; set; }

        public Guid UploadedBy { get; set; }
        public User? Uploader { get; set; }
    }
}
