using EventHub.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHub.Configurations
{
    public class OrganizerProfileConfiguration : IEntityTypeConfiguration<OrganizerProfile>
    {
        public void Configure(EntityTypeBuilder<OrganizerProfile> builder)
        {
            builder.ToTable("OrganizerProfiles");

            builder.HasKey(op => op.Id);

            builder.Property(op => op.Bio)
                   .HasMaxLength(500);

            builder.Property(op => op.WebsiteUrl)
                   .HasMaxLength(250);

            builder.Property(op => op.LogoUrl)
                   .HasMaxLength(250);

            // One-to-One: Organizer <-> OrganizerProfile
            builder.HasOne(op => op.Organizer)
                   .WithOne(o => o.Profile)
                   .HasForeignKey<OrganizerProfile>(op => op.OrganizerId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.HasIndex(op => op.OrganizerId)
                   .IsUnique();
        }
    }
}
