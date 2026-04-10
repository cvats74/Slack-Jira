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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table name in SQL Server
            builder.ToTable("Users");

            // Primary Key
            builder.HasKey(u => u.Id);

            // FirstName — required, max 100 characters
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            // LastName — required, max 100 characters
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            // Email — required, max 256, must be unique
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            // Unique index on Email
            // No two users can have same email
            builder.HasIndex(u => u.Email)
                .IsUnique();

            // PasswordHash — required, no max length
            builder.Property(u => u.PasswordHash)
                .IsRequired();

            // PhoneNumber — optional, max 20
            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            // Store Role enum as integer in database
            // Admin=1, Manager=2, Employee=3
            builder.Property(u => u.Role)
                .HasConversion<int>();

            // Store Status enum as integer
            builder.Property(u => u.Status)
                .HasConversion<int>();

            // Relationship: User belongs to Organization
            // One Organization has many Users
            // Restrict = don't delete users if org deleted
            builder.HasOne(u => u.Organization)
                .WithMany(o => o.Members)
                .HasForeignKey(u => u.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            // FullName is computed in C#
            // Tell EF Core: don't create a column for this
            builder.Ignore(u => u.FullName);
        }
    }
}
