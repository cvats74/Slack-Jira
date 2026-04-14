using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Entities;

namespace WorkFlowPro.Application.Common.Interfaces
{
    public interface IOrganizationRepository
    {
        //find org id
        Task<Organization?> GetByIdAsync(Guid Id);

        //// Find organization by its URL slug
        // eg: "google-india"
        Task<Organization?> GetBySlugAsync(string slug);

        // Check slug availability before creating
        Task<bool> SlugExistsAsync(string slug);

        Task<Organization> CreateAsync(Organization organization);

        Task<int> SaveChangesAsync();
    }
}
