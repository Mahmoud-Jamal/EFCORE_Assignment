using NationalBankManagementSystem.Models.Enums;

namespace NationalBankManagementSystem.Models;

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public CustomerType CustomerType { get; set; }

    // Navigation property: M-to-M with Account via CustomerAccount
    public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new List<CustomerAccount>();
}
