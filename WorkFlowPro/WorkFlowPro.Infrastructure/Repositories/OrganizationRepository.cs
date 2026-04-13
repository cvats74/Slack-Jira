using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Application.Common.Interfaces;
using WorkFlowPro.Domain.Entities;
using WorkFlowPro.Infrastructure.Data;

namespace WorkFlowPro.Infrastructure.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {

        private readonly AppDbContext _context;

        public OrganizationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Organization> CreateAsync(Organization organization)
        {
            organization.Slug = organization.Slug.ToLower().Trim();
             await _context.AddAsync(organization);
            return organization;
        }

        public async Task<Organization?> GetByIdAsync(Guid Id)
        {
            return await _context.Organizations.FirstOrDefaultAsync(o => o.Id == Id);
        }

        public Task<Organization?> GetBySlugAsync(string slug)
        {
            return _context.Organizations.FirstOrDefaultAsync(o => o.Slug == slug.ToLower().Trim());
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task<bool> SlugExistsAsync(string slug)
        {
            return _context.Organizations.AnyAsync(o => o.Slug == slug.ToLower().Trim());
        }
    }
}
