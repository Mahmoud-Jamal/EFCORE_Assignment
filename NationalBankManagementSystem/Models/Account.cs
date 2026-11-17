using NationalBankManagementSystem.Models.Enums;

namespace NationalBankManagementSystem.Models;

public class Account
{
    public string AccountNumber { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public AccountType AccountType { get; set; }
    public DateTime OpeningDate { get; set; }

    // Foreign Key & Navigation to Branch (Belong)
    public string BranchCode { get; set; } = string.Empty;
    public Branch? Branch { get; set; }

    // M-to-M with Customer via CustomerAccount (Have)
    public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new List<CustomerAccount>();

    // 1-to-Many with Transaction (Related)
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
