using NationalBankManagementSystem.Models;
using NationalBankManagementSystem.Models.Enums;
using NationalBankManagementSystem.Services;

namespace NationalBankManagementSystem.UI;

public class BankConsoleMenu
{
    private readonly BankService _bankService;

    public BankConsoleMenu(BankService bankService)
    {
        _bankService = bankService;
    }

    public void Run()
    {
        bool running = true;

        while (running)
        {
            DrawMenu();
            int choice = ConsoleInputHelper.ReadMenuChoice(minChoice: 0, maxChoice: 5);

            switch (choice)
            {
                case 1:
                    HandleAddCustomer();
                    break;
                case 2:
                    HandleOpenAccount();
                    break;
                case 3:
                    HandleUpdateAccountStatus();
                    break;
                case 4:
                    HandleRemoveAccount();
                    break;
                case 5:
                    HandleListAllCustomers();
                    break;
                case 0:
                    running = false;
                    Console.WriteLine("\nExiting National Bank Management System. Goodbye!");
                    break;
            }

            if (running)
            {
                ConsoleInputHelper.Pause();
            }
        }
    }

    private void DrawMenu()
    {
        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("  National Bank - Management");
        Console.WriteLine("========================================");
        Console.WriteLine("1) Add a new Customer");
        Console.WriteLine("2) Open a new Account for a Customer");
        Console.WriteLine("3) Update Account Status (Active / Closed)");
        Console.WriteLine("4) Remove an Account from a Customer");
        Console.WriteLine("5) List all Customers (with accounts)");
        Console.WriteLine("0) Exit");
        Console.WriteLine("----------------------------------------");
    }

    private void HandleAddCustomer()
    {
        Console.WriteLine("\n--- Add New Customer ---");

        string fullName = ConsoleInputHelper.ReadRequiredString("Full Name     : ");
        string nationalId = ConsoleInputHelper.ReadRequiredString("National ID   : ");
        DateOnly dob = ConsoleInputHelper.ReadDateOnly("Date of Birth : (yyyy-MM-dd) ");
        string email = ConsoleInputHelper.ReadEmail("Email         : ");
        string phone = ConsoleInputHelper.ReadRequiredString("Phone         : ");
        string address = ConsoleInputHelper.ReadRequiredString("Address       : ");

        Console.WriteLine("Customer Type:");
        Console.WriteLine("  1) Individual");
        Console.WriteLine("  2) Business");
        int typeChoice = ConsoleInputHelper.ReadInt("Choice: ", min: 1, max: 2);
        CustomerType customerType = (CustomerType)typeChoice;

        var customer = new Customer
        {
            FullName = fullName,
            NationalId = nationalId,
            DateOfBirth = dob,
            Email = email,
            PhoneNumber = phone,
            Address = address,
            CustomerType = customerType
        };

        var (success, message, customerId) = _bankService.AddCustomer(customer);
        if (success)
        {
            Console.WriteLine();
            ConsoleInputHelper.PrintSuccess(message);
        }
        else
        {
            Console.WriteLine();
            ConsoleInputHelper.PrintError(message);
        }
    }

    private void HandleOpenAccount()
    {
        Console.WriteLine("\n--- Open New Account ---");

        string accountNumber = ConsoleInputHelper.ReadRequiredString("Account Number : ");

        Console.WriteLine("Account Type:");
        Console.WriteLine("  1) Savings");
        Console.WriteLine("  2) Current");
        Console.WriteLine("  3) Business");
        int typeChoice = ConsoleInputHelper.ReadInt("Choice: ", min: 1, max: 3);
        AccountType accountType = (AccountType)typeChoice;

        string branchCode = ConsoleInputHelper.ReadRequiredString("Branch Code    : ");
        int customerId = ConsoleInputHelper.ReadInt("Customer Id    : ", min: 1);

        Console.WriteLine("Ownership Role:");
        Console.WriteLine("  1) Primary");
        Console.WriteLine("  2) CoHolder");
        int roleChoice = ConsoleInputHelper.ReadInt("Choice: ", min: 1, max: 2);
        OwnershipType ownershipRole = (OwnershipType)roleChoice;

        ConsoleInputHelper.PrintInfo($"Validating branch '{branchCode}' and customer #{customerId}...");

        var (success, message) = _bankService.OpenAccount(accountNumber, accountType, branchCode, customerId, ownershipRole);
        if (success)
        {
            ConsoleInputHelper.PrintSuccess(message);
        }
        else
        {
            ConsoleInputHelper.PrintError(message);
        }
    }

    private void HandleUpdateAccountStatus()
    {
        Console.WriteLine("\n--- Update Account Status ---");

        string accountNumber = ConsoleInputHelper.ReadRequiredString("Account Number : ");
        int customerId = ConsoleInputHelper.ReadInt("Customer Id    : ", min: 1);

        Console.WriteLine("New Status:");
        Console.WriteLine("  1) Active");
        Console.WriteLine("  2) Closed");
        int statusChoice = ConsoleInputHelper.ReadInt("Choice: ", min: 1, max: 2);
        AccountStatus newStatus = (AccountStatus)statusChoice;

        var (success, message) = _bankService.UpdateAccountStatus(accountNumber, customerId, newStatus);
        if (success)
        {
            ConsoleInputHelper.PrintSuccess(message);
        }
        else
        {
            ConsoleInputHelper.PrintError(message);
        }
    }

    private void HandleRemoveAccount()
    {
        Console.WriteLine("\n--- Remove Account From Customer ---");

        string accountNumber = ConsoleInputHelper.ReadRequiredString("Account Number : ");
        int customerId = ConsoleInputHelper.ReadInt("Customer Id    : ", min: 1);

        var (success, message, _) = _bankService.RemoveAccountFromCustomer(accountNumber, customerId);
        Console.WriteLine();
        if (success)
        {
            ConsoleInputHelper.PrintSuccess(message);
        }
        else
        {
            ConsoleInputHelper.PrintError(message);
        }
    }

    private void HandleListAllCustomers()
    {
        Console.WriteLine("\n--- All Customers ---");

        var customers = _bankService.GetAllCustomersWithAccounts();

        if (customers.Count == 0)
        {
            Console.WriteLine("  (No customers found in database)");
            return;
        }

        foreach (var customer in customers)
        {
            Console.WriteLine($"\n#{customer.Id} {customer.FullName} ({customer.CustomerType})");

            if (customer.CustomerAccounts.Count == 0)
            {
                Console.WriteLine("    (no accounts)");
            }
            else
            {
                foreach (var ca in customer.CustomerAccounts)
                {
                    var acc = ca.Account;
                    var branchName = acc?.Branch?.Name ?? "Unknown Branch";
                    var balanceStr = acc != null ? $"{acc.CurrentBalance,12:N2}" : "        0.00";
                    var accNum = acc?.AccountNumber ?? ca.AccountNumber;
                    var accType = acc?.AccountType.ToString() ?? "Unknown";

                    Console.WriteLine($"    {accNum,-10} {accType,-9} Balance: {balanceStr}   {ca.OwnershipType,-9} {ca.AccountStatus,-8} @ {branchName}");
                }
            }
        }
    }
}
