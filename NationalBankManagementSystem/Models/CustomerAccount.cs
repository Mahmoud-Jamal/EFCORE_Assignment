using NationalBankManagementSystem.Models.Enums;

namespace NationalBankManagementSystem.Models;

public class CustomerAccount
{
    // Composite Primary Key: CustomerId + AccountNumber
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public string AccountNumber { get; set; } = string.Empty;
    public Account? Account { get; set; }

    public OwnershipType OwnershipType { get; set; }
    public DateTime OwnershipStartDate { get; set; }
    public AccountStatus AccountStatus { get; set; }
}
