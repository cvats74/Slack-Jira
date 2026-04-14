using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Entities;

namespace WorkFlowPro.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        // Find a user by their email address
        // Returns null if not found
        Task<User?> GetByEmailAsync (string email);

        //find user by id 
        Task<User?> GetByIdAsync (Guid id);

        //get all users in organisation
        Task<IEnumerable<User>> GetByOrganizationAsync (Guid orgaizationId);

        //check if user exists/ prevent duplicaates
        Task<bool> EmailExistsAsync(string email);

        //create user in db
        Task<User> CreateAsync(User user);

        //update user
        Task<User> UpdateAsync(User user);

        //save changes in db
        Task<int> SaveChangesAsync();
    }
}
