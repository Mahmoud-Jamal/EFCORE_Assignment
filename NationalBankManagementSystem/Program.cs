using Microsoft.EntityFrameworkCore;
using NationalBankManagementSystem.Data;
using NationalBankManagementSystem.Services;
using NationalBankManagementSystem.UI;

namespace NationalBankManagementSystem;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // 1. Ensure Database is created and migrations are applied
        using (var context = new BankDbContext())
        {
            context.Database.Migrate();

            // 2. Part 02 Seeding: Ensure sample data matching expected outputs is present
            BankDbSeeder.SeedSampleData(context);
        }

        // 3. Part 03: Run Interactive Console Menu
        var bankService = new BankService();
        var menu = new BankConsoleMenu(bankService);
        menu.Run();
    }
}
