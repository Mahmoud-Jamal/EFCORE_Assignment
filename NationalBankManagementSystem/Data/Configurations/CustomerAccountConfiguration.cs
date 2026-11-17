using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NationalBankManagementSystem.Models;

namespace NationalBankManagementSystem.Data.Configurations;

public class CustomerAccountConfiguration : IEntityTypeConfiguration<CustomerAccount>
{
    public void Configure(EntityTypeBuilder<CustomerAccount> builder)
    {
        builder.ToTable("CustomerAccounts");

        // Composite Primary Key
        builder.HasKey(ca => new { ca.CustomerId, ca.AccountNumber });

        builder.Property(ca => ca.AccountNumber)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(ca => ca.OwnershipType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(ca => ca.AccountStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(ca => ca.OwnershipStartDate)
            .IsRequired();

        // Foreign Key to Customer
        builder.HasOne(ca => ca.Customer)
            .WithMany(c => c.CustomerAccounts)
            .HasForeignKey(ca => ca.CustomerId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Foreign Key to Account
        builder.HasOne(ca => ca.Account)
            .WithMany(a => a.CustomerAccounts)
            .HasForeignKey(ca => ca.AccountNumber)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
