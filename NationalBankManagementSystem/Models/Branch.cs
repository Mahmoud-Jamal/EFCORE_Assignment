namespace NationalBankManagementSystem.Models;

public class Branch
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    // 1-to-1: Branch <-> Manager
    public Manager? Manager { get; set; }

    // 1-to-Many: Branch <-> Accounts
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
