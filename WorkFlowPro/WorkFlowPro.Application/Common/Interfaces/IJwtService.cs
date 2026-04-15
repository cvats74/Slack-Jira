using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Entities;

namespace WorkFlowPro.Application.Common.Interfaces
{
                                               // Single Responsibility Principle(one class do one thing)
    public interface IJwtService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        DateTime GetTokenExpiry();
    }
}
