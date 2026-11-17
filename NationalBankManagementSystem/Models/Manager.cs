namespace NationalBankManagementSystem.Models;

public class Manager
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; }

    // Foreign Key & Navigation to Branch (1-to-1)
    public string BranchCode { get; set; } = string.Empty;
    public Branch? Branch { get; set; }
}
