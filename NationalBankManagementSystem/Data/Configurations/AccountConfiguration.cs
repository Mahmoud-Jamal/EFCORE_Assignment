using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NationalBankManagementSystem.Models;

namespace NationalBankManagementSystem.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts", t =>
        {
            t.HasCheckConstraint("CK_Accounts_CurrentBalance", "[CurrentBalance] >= 0");
        });

        builder.HasKey(a => a.AccountNumber);

        builder.Property(a => a.AccountNumber)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(a => a.CurrentBalance)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasDefaultValue(0m);

        builder.Property(a => a.AccountType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(a => a.OpeningDate)
            .IsRequired();

        builder.Property(a => a.BranchCode)
            .IsRequired()
            .HasMaxLength(20);

        // One-to-Many: Branch <-> Account
        builder.HasOne(a => a.Branch)
            .WithMany(b => b.Accounts)
            .HasForeignKey(a => a.BranchCode)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
