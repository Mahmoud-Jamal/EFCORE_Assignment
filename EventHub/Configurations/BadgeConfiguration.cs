using EventHub.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHub.Configurations
{
    public class BadgeConfiguration : IEntityTypeConfiguration<Badge>
    {
        public void Configure(EntityTypeBuilder<Badge> builder)
        {
            builder.ToTable("Badges");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.BadgeNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(b => b.IssueDate)
                   .IsRequired();

            builder.Property(b => b.Tier)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20);

            // One-to-One: Attendee <-> Badge
            builder.HasOne(b => b.Attendee)
                   .WithOne(a => a.Badge)
                   .HasForeignKey<Badge>(b => b.AttendeeId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            // Enforce uniqueness on the foreign key for true 1-to-1
            builder.HasIndex(b => b.AttendeeId)
                   .IsUnique();
        }
    }
}
