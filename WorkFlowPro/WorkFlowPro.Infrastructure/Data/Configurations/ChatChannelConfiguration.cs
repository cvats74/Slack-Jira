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
    public class ChatChannelConfiguration : IEntityTypeConfiguration<ChatChannel>

    {
        public void Configure(EntityTypeBuilder<ChatChannel> builder)
        {
            builder.ToTable("ChatChannels");

            builder.HasKey(cc => cc.Id);

            builder.Property(cc => cc.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(cc => cc.Description)
                .HasMaxLength(500);

            // Channel belongs to a Project
            builder.HasOne(cc => cc.Project)
                .WithMany(p => p.ChatChannels)
                .HasForeignKey(cc => cc.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
