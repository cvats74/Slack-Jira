using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowPro.Domain.Common
{
    public abstract class BaseEntity
    {

        protected BaseEntity() {
            
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }

        public Guid Id { get; set; }

        //multi-tenant id

        public  Guid TenantId {  get; set; }

        //audit
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        //soft delete

        public bool IsDeleted { get; set; } = false;
        public Guid DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }


    }
}
