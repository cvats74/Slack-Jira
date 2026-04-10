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
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.ToTable("ChatMessages");

            builder.HasKey(cm => cm.Id);

            builder.Property(cm => cm.Content)
                .IsRequired()
                .HasMaxLength(4000);

            // Message belongs to a Channel
            builder.HasOne(cm => cm.Channel)
                .WithMany(cc => cc.Messages)
                .HasForeignKey(cm => cm.ChannelId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message sent by a User
            builder.HasOne(cm => cm.Sender)
                .WithMany()
                .HasForeignKey(cm => cm.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
