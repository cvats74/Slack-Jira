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
    public class WorkItemConfiguration: IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> builder)
        {
            builder.ToTable("WorkItems");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Title)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(w => w.Description)
                .HasMaxLength(5000);

            builder.Property(w => w.Status)
                .HasConversion<int>();

            builder.Property(w => w.Priority)
                .HasConversion<int>();

            // WorkItem belongs to a Project
            builder.HasOne(w => w.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(w => w.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // WorkItem can be assigned to a User
            // AssigneeId is nullable (unassigned tasks)
            builder.HasOne(w => w.Assignee)
                .WithMany()
                .HasForeignKey(w => w.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            // WorkItem has a Reporter (who created it)
            builder.HasOne(w => w.Reporter)
                .WithMany()
                .HasForeignKey(w => w.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Self-referencing: WorkItem has SubTasks
            // ParentTask contains many SubTasks
            // ParentTaskId is nullable (top-level tasks)
            builder.HasOne(w => w.ParentTask)
                .WithMany(w => w.SubTasks)
                .HasForeignKey(w => w.ParentTaskId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
