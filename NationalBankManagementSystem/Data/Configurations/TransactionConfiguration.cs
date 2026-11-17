using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NationalBankManagementSystem.Models;

namespace NationalBankManagementSystem.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions", t =>
        {
            t.HasCheckConstraint("CK_Transactions_Amount", "[Amount] > 0");
        });

        builder.HasKey(t => t.TransactionNumber);

        builder.Property(t => t.TransactionNumber)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(t => t.TransactionDate)
            .IsRequired();

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(t => t.Note)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(t => t.TransactionType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(t => t.AccountNumber)
            .IsRequired()
            .HasMaxLength(30);

        // One-to-Many: Account <-> Transactions (Cascade delete transactions when account is removed)
        builder.HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountNumber)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
