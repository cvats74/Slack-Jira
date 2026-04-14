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
    public class UserRepository :IUserRepository
    {

        // This is Dependency Injection in action
        private readonly AppDbContext _context;                           
        public UserRepository(AppDbContext context) {

            _context = context;
        }

        public async Task<User> CreateAsync(User user)
        {
            user.Email = user.Email.ToLower().Trim();
            await _context.AddAsync(user);
            return user;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
           return await _context.Users.AnyAsync(u => u.Email == email.ToLower().Trim());
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower().Trim());
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync( i => i.Id == id);
        }

        public async Task<IEnumerable<User>> GetByOrganizationAsync(Guid orgaizationId)
        {
            return await _context.Users.Where(u => u.OrganizationId == orgaizationId).ToListAsync();
        }



        public Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return Task.FromResult(user);
        }

        public async Task<int> SaveChangesAsync()
        {
            // Actually execute SQL against database
            // Returns number of rows affected
            return await _context.SaveChangesAsync();
        }

       
    }
}
