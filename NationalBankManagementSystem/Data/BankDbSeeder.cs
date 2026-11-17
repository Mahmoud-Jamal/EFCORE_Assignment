using NationalBankManagementSystem.Models;
using NationalBankManagementSystem.Models.Enums;

namespace NationalBankManagementSystem.Data;

public static class BankDbSeeder
{
    public static void SeedSampleData(BankDbContext context)
    {
        if (context.Customers.Any() || context.Accounts.Any())
        {
            return;
        }

        // 1. Initial Customers matching sample expected outputs
        var customer1 = new Customer
        {
            FullName = "Ahmed Ali",
            NationalId = "29501011234567",
            DateOfBirth = new DateOnly(1995, 1, 1),
            Email = "ahmed.ali@example.com",
            PhoneNumber = "01012345678",
            Address = "10 Ramses Street, Cairo, Egypt",
            CustomerType = CustomerType.Individual
        };

        var customer2 = new Customer
        {
            FullName = "Nile Trading LLC",
            NationalId = "78912345600012",
            DateOfBirth = new DateOnly(2010, 6, 15),
            Email = "info@niletrading.com",
            PhoneNumber = "01287654321",
            Address = "Plot 4, Free Industrial Zone, Alexandria, Egypt",
            CustomerType = CustomerType.Business
        };

        context.Customers.AddRange(customer1, customer2);
        context.SaveChanges();

        // 2. Initial Accounts matching sample outputs
        var account1 = new Account
        {
            AccountNumber = "2001-CUR",
            CurrentBalance = 15400.00m,
            AccountType = AccountType.Current,
            OpeningDate = new DateTime(2023, 1, 10, 9, 0, 0, DateTimeKind.Utc),
            BranchCode = "CAI-01"
        };

        var account2 = new Account
        {
            AccountNumber = "3000-BUS",
            CurrentBalance = 120000.00m,
            AccountType = AccountType.Business,
            OpeningDate = new DateTime(2023, 3, 20, 11, 30, 0, DateTimeKind.Utc),
            BranchCode = "ALX-01"
        };

        context.Accounts.AddRange(account1, account2);
        context.SaveChanges();

        // 3. CustomerAccount relationships
        var ca1 = new CustomerAccount
        {
            CustomerId = customer1.Id,
            AccountNumber = account1.AccountNumber,
            OwnershipType = OwnershipType.Primary,
            OwnershipStartDate = new DateTime(2023, 1, 10, 9, 0, 0, DateTimeKind.Utc),
            AccountStatus = AccountStatus.Active
        };

        var ca2 = new CustomerAccount
        {
            CustomerId = customer2.Id,
            AccountNumber = account2.AccountNumber,
            OwnershipType = OwnershipType.Primary,
            OwnershipStartDate = new DateTime(2023, 3, 20, 11, 30, 0, DateTimeKind.Utc),
            AccountStatus = AccountStatus.Active
        };

        context.CustomerAccounts.AddRange(ca1, ca2);

        // 4. Sample Transactions
        var tx1 = new Transaction
        {
            TransactionNumber = "TX-20230110-001",
            TransactionDate = new DateTime(2023, 1, 10, 9, 5, 0, DateTimeKind.Utc),
            Amount = 15400.00m,
            Note = "Initial account opening deposit",
            TransactionType = TransactionType.Deposit,
            AccountNumber = account1.AccountNumber
        };

        var tx2 = new Transaction
        {
            TransactionNumber = "TX-20230320-001",
            TransactionDate = new DateTime(2023, 3, 20, 11, 35, 0, DateTimeKind.Utc),
            Amount = 120000.00m,
            Note = "Initial corporate opening deposit",
            TransactionType = TransactionType.Deposit,
            AccountNumber = account2.AccountNumber
        };

        context.Transactions.AddRange(tx1, tx2);
        context.SaveChanges();
    }
}
