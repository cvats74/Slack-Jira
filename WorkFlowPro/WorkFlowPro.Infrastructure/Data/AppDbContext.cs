using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Entities;

namespace WorkFlowPro.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
           

            
        }

        //tables in sql server
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ChatChannel> ChatChannels { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<FileAttachment> FileAttachments { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected AppDbContext()
        {
        }
    }
}
