using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NationalBankManagementSystem.Models;

namespace NationalBankManagementSystem.Data;

public class BankDbContext : DbContext
{
    public BankDbContext()
    {
    }

    public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
    {
    }

    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Manager> Managers => Set<Manager>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<CustomerAccount> CustomerAccounts => Set<CustomerAccount>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection")
                ?? "Server=Mahmoudgamal\\SQLEXPRESS;Database=NationalBankManagementSystem;Trusted_Connection=True;TrustServerCertificate=true;";

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankDbContext).Assembly);
    }
}
