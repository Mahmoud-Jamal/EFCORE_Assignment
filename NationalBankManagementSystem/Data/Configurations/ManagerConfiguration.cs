using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NationalBankManagementSystem.Models;

namespace NationalBankManagementSystem.Data.Configurations;

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.ToTable("Managers");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(m => m.Email)
            .IsUnique();

        builder.Property(m => m.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(m => m.HireDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(m => m.BranchCode)
            .IsRequired()
            .HasMaxLength(20);

        // 1-to-1: Manager <-> Branch
        builder.HasOne(m => m.Branch)
            .WithOne(b => b.Manager)
            .HasForeignKey<Manager>(m => m.BranchCode)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasIndex(m => m.BranchCode)
            .IsUnique();

        // Seed Data: 3 Managers
        builder.HasData(
            new Manager
            {
                Id = 1,
                FullName = "Tarek Mansour",
                Email = "tarek.mansour@nbg.com",
                PhoneNumber = "01011122334",
                HireDate = new DateOnly(2018, 5, 10),
                BranchCode = "CAI-01"
            },
            new Manager
            {
                Id = 2,
                FullName = "Dalia Fawzy",
                Email = "dalia.fawzy@nbg.com",
                PhoneNumber = "01122233445",
                HireDate = new DateOnly(2020, 2, 1),
                BranchCode = "ALX-01"
            },
            new Manager
            {
                Id = 3,
                FullName = "Hossam Zaki",
                Email = "hossam.zaki@nbg.com",
                PhoneNumber = "01233344556",
                HireDate = new DateOnly(2021, 9, 15),
                BranchCode = "GIZ-01"
            }
        );
    }
}
