using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NationalBankManagementSystem.Models;

namespace NationalBankManagementSystem.Data.Configurations;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");

        builder.HasKey(b => b.Code);

        builder.Property(b => b.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(b => b.Address)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(b => b.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        // One-to-Many: Branch <-> Accounts
        builder.HasMany(b => b.Accounts)
            .WithOne(a => a.Branch)
            .HasForeignKey(a => a.BranchCode)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Seed Data: 3 Branches
        builder.HasData(
            new Branch
            {
                Code = "CAI-01",
                Name = "Cairo Main Branch",
                Address = "15 El Tahrir Square, Downtown, Cairo",
                PhoneNumber = "02-25789001"
            },
            new Branch
            {
                Code = "ALX-01",
                Name = "Alexandria Branch",
                Address = "42 Corniche Road, Ramleh, Alexandria",
                PhoneNumber = "03-4861234"
            },
            new Branch
            {
                Code = "GIZ-01",
                Name = "Giza Smart Village Branch",
                Address = "Building B-12, Smart Village, Giza",
                PhoneNumber = "02-35345678"
            }
        );
    }
}
