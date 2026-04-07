using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowPro.Domain.Enums
{
    public enum TaskStatus
    {
        ToDo = 1,
        InProgress = 2,
        InReview = 3,
        Blocked = 4,
        Done = 5,
        Cancelled = 6
    }
}
