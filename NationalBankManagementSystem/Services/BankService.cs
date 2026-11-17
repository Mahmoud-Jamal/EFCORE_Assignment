using Microsoft.EntityFrameworkCore;
using NationalBankManagementSystem.Data;
using NationalBankManagementSystem.Models;
using NationalBankManagementSystem.Models.Enums;

namespace NationalBankManagementSystem.Services;

public class BankService
{
    /// <summary>
    /// Checks whether a branch exists by its code.
    /// </summary>
    public bool BranchExists(string branchCode)
    {
        using var context = new BankDbContext();
        return context.Branches.Any(b => b.Code == branchCode);
    }

    /// <summary>
    /// Checks whether a customer exists by their Id.
    /// </summary>
    public bool CustomerExists(int customerId)
    {
        using var context = new BankDbContext();
        return context.Customers.Any(c => c.Id == customerId);
    }

    /// <summary>
    /// Checks whether an account exists by its number.
    /// </summary>
    public bool AccountExists(string accountNumber)
    {
        using var context = new BankDbContext();
        return context.Accounts.Any(a => a.AccountNumber == accountNumber);
    }

    /// <summary>
    /// Adds a new customer to the system after validating uniqueness of NationalId and Email.
    /// </summary>
    public (bool Success, string Message, int? CustomerId) AddCustomer(Customer customer)
    {
        using var context = new BankDbContext();

        if (context.Customers.Any(c => c.NationalId == customer.NationalId))
        {
            return (false, $"Customer with National ID '{customer.NationalId}' already exists.", null);
        }

        if (context.Customers.Any(c => c.Email == customer.Email))
        {
            return (false, $"Customer with Email '{customer.Email}' already exists.", null);
        }

        context.Customers.Add(customer);
        context.SaveChanges();

        return (true, $"Customer created successfully. CustomerId = {customer.Id}", customer.Id);
    }

    /// <summary>
    /// Opens a new account or links a customer to an existing account.
    /// Verifies that branch and customer exist before creating/linking.
    /// </summary>
    public (bool Success, string Message) OpenAccount(
        string accountNumber,
        AccountType accountType,
        string branchCode,
        int customerId,
        OwnershipType ownershipRole)
    {
        using var context = new BankDbContext();

        var branch = context.Branches.Find(branchCode);
        if (branch == null)
        {
            return (false, $"Branch with Code '{branchCode}' does not exist.");
        }

        var customer = context.Customers.Find(customerId);
        if (customer == null)
        {
            return (false, $"Customer with Id #{customerId} does not exist.");
        }

        var existingAccount = context.Accounts.Find(accountNumber);
        if (existingAccount == null)
        {
            // Create new Account
            var newAccount = new Account
            {
                AccountNumber = accountNumber,
                AccountType = accountType,
                BranchCode = branchCode,
                OpeningDate = DateTime.UtcNow,
                CurrentBalance = 0m
            };
            context.Accounts.Add(newAccount);

            // Create initial CustomerAccount link
            var customerAccount = new CustomerAccount
            {
                CustomerId = customerId,
                AccountNumber = accountNumber,
                OwnershipType = ownershipRole,
                OwnershipStartDate = DateTime.UtcNow,
                AccountStatus = AccountStatus.Active
            };
            context.CustomerAccounts.Add(customerAccount);

            context.SaveChanges();
            return (true, $"Account '{accountNumber}' created and linked to customer {customerId} as {ownershipRole} owner.");
        }
        else
        {
            // Account already exists: check if this customer is already linked
            var existingLink = context.CustomerAccounts.Find(customerId, accountNumber);
            if (existingLink != null)
            {
                return (false, $"Customer #{customerId} is ALREADY linked to account '{accountNumber}'.");
            }

            // Link as CoHolder or Primary
            var customerAccount = new CustomerAccount
            {
                CustomerId = customerId,
                AccountNumber = accountNumber,
                OwnershipType = ownershipRole,
                OwnershipStartDate = DateTime.UtcNow,
                AccountStatus = AccountStatus.Active
            };
            context.CustomerAccounts.Add(customerAccount);
            context.SaveChanges();

            return (true, $"Existing account '{accountNumber}' linked to customer {customerId} as {ownershipRole} owner.");
        }
    }

    /// <summary>
    /// Updates the status of an account for a given customer.
    /// </summary>
    public (bool Success, string Message) UpdateAccountStatus(string accountNumber, int customerId, AccountStatus newStatus)
    {
        using var context = new BankDbContext();

        var customerAccount = context.CustomerAccounts.Find(customerId, accountNumber);
        if (customerAccount == null)
        {
            return (false, $"No account relationship found for Customer #{customerId} and Account '{accountNumber}'.");
        }

        customerAccount.AccountStatus = newStatus;
        context.SaveChanges();

        return (true, $"Status updated to {newStatus}.");
    }

    /// <summary>
    /// Removes an account link from a customer.
    /// If that was the last customer linked to the account, the account is also removed.
    /// </summary>
    public (bool Success, string Message, bool AccountAlsoDeleted) RemoveAccountFromCustomer(string accountNumber, int customerId)
    {
        using var context = new BankDbContext();

        var customerAccount = context.CustomerAccounts.Find(customerId, accountNumber);
        if (customerAccount == null)
        {
            return (false, $"Customer #{customerId} is not associated with Account '{accountNumber}'.", false);
        }

        context.CustomerAccounts.Remove(customerAccount);
        context.SaveChanges();

        // Check if there are remaining owners
        bool hasOtherOwners = context.CustomerAccounts.Any(ca => ca.AccountNumber == accountNumber);
        bool accountDeleted = false;

        if (!hasOtherOwners)
        {
            var account = context.Accounts.Find(accountNumber);
            if (account != null)
            {
                context.Accounts.Remove(account);
                context.SaveChanges();
                accountDeleted = true;
            }
        }

        string msg = "Ownership link deleted.";
        if (accountDeleted)
        {
            msg += $"\n  That was the last owner - account '{accountNumber}' was also removed.";
        }

        return (true, msg, accountDeleted);
    }

    /// <summary>
    /// Uses Eager Loading (Include + ThenInclude) to retrieve all customers along with their
    /// CustomerAccount relationships, the underlying Account details, and the Branch information.
    /// </summary>
    public List<Customer> GetAllCustomersWithAccounts()
    {
        using var context = new BankDbContext();

        return context.Customers
            .AsNoTracking()
            .Include(c => c.CustomerAccounts)
                .ThenInclude(ca => ca.Account)
                    .ThenInclude(a => a!.Branch)
            .OrderBy(c => c.Id)
            .ToList();
    }
}
