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
    public class FileAttachmentConfiguration: IEntityTypeConfiguration<FileAttachment>
    {
        public void Configure(EntityTypeBuilder<FileAttachment> builder)
        {
            builder.ToTable("FileAttachments");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.FileName)
                .IsRequired()
                .HasMaxLength(255);

            // URL where file is stored
            builder.Property(f => f.FileUrl)
                .IsRequired()
                .HasMaxLength(1000);

            // MIME type: image/png, application/pdf etc
            builder.Property(f => f.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            // File belongs to a WorkItem
            builder.HasOne(f => f.WorkItem)
                .WithMany(w => w.Attachments)
                .HasForeignKey(f => f.WorkItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // File uploaded by a User
            builder.HasOne(f => f.Uploader)
                .WithMany()
                .HasForeignKey(f => f.UploadedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
