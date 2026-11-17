using NationalBankManagementSystem.Models.Enums;

namespace NationalBankManagementSystem.Models;

public class Transaction
{
    public string TransactionNumber { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public TransactionType TransactionType { get; set; }

    // Foreign Key & Navigation to Account
    public string AccountNumber { get; set; } = string.Empty;
    public Account? Account { get; set; }
}
