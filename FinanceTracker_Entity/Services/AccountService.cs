using FinanceTracker_Entity.Data;
using FinanceTracker_Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinanceTracker_Entity.Models.Enums;

namespace FinanceTracker_Entity.Services
{
    public class AccountService
    {

        //UserValidator _validator = new UserValidator();
        private DataContext _context;

        public AccountService(DataContext context)
        {
            _context = context;
        }

        public void CreateAccount()
        {

            var user = DataContext.CurrentUser!;

            Console.WriteLine("\n=== Create Account ===");

            Console.Write("Name: ");

            string name = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name is required.");
                name = Console.ReadLine();
            }

            Console.Write("Type (Cash/Bank/Card): ");
            string input = (Console.ReadLine() ?? "").Trim();

            AccountType accountType;
            while (!Enum.TryParse(input, true, out accountType))
            {
                Console.WriteLine("Invalid type. Please enter Cash, Bank, or Card:");
                input = (Console.ReadLine() ?? "").Trim();
            }

            Console.WriteLine("Please indicate the currency: ");
            var ccy = Console.ReadLine();

            var newAccount = new Account()
            {
                AccountId = Guid.NewGuid(),
                Name = name,
                Type = accountType,
                Currency = ccy,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
            };

            _context.Accounts.Add(newAccount);
            _context.SaveChanges();

            Console.WriteLine("✅ Account created successfully.");
            Console.WriteLine($"{newAccount.AccountId}, " + $"Account Type: {newAccount.Type}, " + $"Currency: {newAccount.Currency}");
        }
        public void ListAccounts()
        {
            var userId = DataContext.CurrentUser!.Id;
            var rows = _context.Accounts
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.Name)
                .ToList();

            if (rows.Count == 0)
            {
                Console.WriteLine("\n(No accounts yet)");
                return;
            }

            Console.WriteLine("\nId   Name                 Type    Currency   Balance       Created (UTC)");
            Console.WriteLine(new string('-', 80));
            foreach (var a in rows)
            {
                Console.WriteLine($"{a.AccountId,-4} {a.Name,-20} {a.Type,-7}  {a.Currency,-8}  {a.Balance,10:F2}   {a.CreatedDate:yyyy-MM-dd}");
            }
        }
        public void RenameAccount()
        {
            var user = DataContext.CurrentUser!;
            ListAccounts();

            Console.Write("\nEnter Account Id to rename: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid Id.");
                return;
            }

            var acc = _context.Accounts.FirstOrDefault(a => a.AccountId == id && a.Id == user.Id);
            if (acc == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            Console.Write($"New name for '{acc.Name}': ");
            var newName = (Console.ReadLine() ?? "").Trim();

            while (string.IsNullOrWhiteSpace(newName))
            {
                Console.WriteLine("Name cannot be empty.");
                newName = Console.ReadLine();
            }

            acc.Name = newName;
            _context.SaveChanges();
            Console.WriteLine("Renamed successfully.");
        }
        public void DeleteAccount()
        {
            {
                var user = DataContext.CurrentUser!;
                ListAccounts();

                Console.Write("\nEnter Account Id to delete: ");
                if (!int.TryParse(Console.ReadLine(), out int Id))
                {
                    Console.WriteLine("Invalid Id.");
                    return;
                }

                var acc = _context.Accounts.FirstOrDefault(a => a.AccountId == Id && a.UserId == user.Id);
                if (acc == null)
                {
                    Console.WriteLine("Account not found.");
                    return;
                }

                if (acc.Balance != 0m)
                {
                    Console.WriteLine($"Balance must be 0 to delete. Current balance: {acc.Balance:F2} {acc.Currency}");
                    return;
                }

                Console.Write($"Confirm delete '{acc.Name}'? (y/N): ");
                var confirm = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
                if (confirm != "y")
                {
                    Console.WriteLine("Cancelled.");
                    return;
                }

                _context.Accounts.Remove(acc);
                _context.SaveChanges();
                Console.WriteLine("Deleted successfully.");
            }

        }
    }
}
