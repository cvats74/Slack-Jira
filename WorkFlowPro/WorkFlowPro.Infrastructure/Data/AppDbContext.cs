using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Common;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly());

            // Global soft delete filter
            foreach (var entityType in
                modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity)
                    .IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(
                            BuildSoftDeleteFilter(
                                entityType.ClrType));
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.IsDeleted = false;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }

            return await base.SaveChangesAsync(
                cancellationToken);
        }

        private static LambdaExpression BuildSoftDeleteFilter(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var property = Expression.Property(
                parameter, "IsDeleted");
            var falseConstant = Expression.Constant(false);
            var condition = Expression.Equal(
                property, falseConstant);
            return Expression.Lambda(condition, parameter);
        }
    }
}
