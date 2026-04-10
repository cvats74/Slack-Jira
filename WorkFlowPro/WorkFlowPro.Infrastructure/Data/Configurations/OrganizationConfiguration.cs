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
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
           
            builder.ToTable("Organizations");

            builder.HasKey(o => o.Id);

            // Company name — required
            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(200);

            // URL-friendly name — required, unique
            // google-india, razorpay-hq etc
            builder.Property(o => o.Slug)
                .IsRequired()
                .HasMaxLength(100);

            // No two organizations same slug
            builder.HasIndex(o => o.Slug)
                .IsUnique();

            builder.Property(o => o.Website)
                .HasMaxLength(500);

            builder.Property(o => o.LogoUrl)
                .HasMaxLength(1000);
        }
    }
}
