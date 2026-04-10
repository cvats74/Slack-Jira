using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Domain.Entities;

namespace WorkFlowPro.Infrastructure.Data.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(a => a.Id);

            // Which entity was changed eg "User", "Project"
            builder.Property(a => a.EntityName)
                .IsRequired()
                .HasMaxLength(100);

            // The Id of that entity
            builder.Property(a => a.EntityId)
                .IsRequired()
                .HasMaxLength(50);

            // What happened: "Created", "Updated", "Deleted"
            builder.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(50);

            // JSON of old values before change
            builder.Property(a => a.OldValues)
                .HasMaxLength(4000);

            // JSON of new values after change
            builder.Property(a => a.NewValues)
                .HasMaxLength(4000);

            builder.Property(a => a.IpAddress)
                .HasMaxLength(45);

            // Log belongs to a User who made the change
            builder.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
