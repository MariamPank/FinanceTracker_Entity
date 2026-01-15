using FinanceTracker_Entity.Data;
using FinanceTracker_Entity.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinanceTracker_Entity.Models.Enums;

namespace FinanceTracker_Entity.Utilities
{
    public class ConsoleHelper
    {
        private DataContext _context;

        public ConsoleHelper(DataContext context)
        {
            _context = context;
        }

        public Account PickAccount(int userId)
        {
            var rows = _context.Accounts
                .Where(a => a.Id == userId && a.IsActive)
                .OrderBy(a => a.Name)
                .ToList();

            if (rows.Count == 0)
            {
                Console.WriteLine("No accounts. Please create one first.");
                return null;
            }

            Console.WriteLine("\nYour accounts:");
            Console.WriteLine("Id                                   Name                 Type     Currency   Balance");
            Console.WriteLine(new string('-', 90));
            foreach (var a in rows)
                Console.WriteLine($"{a.Id}  {a.Name,-20} {a.Type,-7}  {a.Currency,-8}  {a.Balance,10:F2}");

            Console.Write("Enter Account Id: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid Id.");
                return null;
            }

            var acc = rows.FirstOrDefault(a => a.UserId == id);
            if (acc == null) Console.WriteLine("Account not found.");
            return acc;
        }

        public decimal ReadNonNegativeAmount(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out var amt) && amt >= 0m)
                    return amt;
                Console.WriteLine("Invalid amount. Please enter a non-negative number.");
            }
        }

        public void ReverseBalanceFor(Transaction tx)
        {
            var acc = _context.Accounts.FirstOrDefault(a => a.Id == tx.AccountId && a.UserId == tx.UserId);
            if (acc == null) return;

            switch (tx.Type)
            {
                case TransactionType.Income:
                case TransactionType.TransferIn:
                    acc.Balance -= tx.Amount;
                    break;

                case TransactionType.Expense:
                case TransactionType.TransferOut:
                    acc.Balance += tx.Amount;
                    break;
            }
        }

        public string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = (Console.ReadLine() ?? "").Trim();
                if (!string.IsNullOrWhiteSpace(s)) return s;
                Console.WriteLine("Value is required.");
            }
        }

        public int ReadIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out var n) && n >= min && n <= max)
                    return n;
                Console.WriteLine($"Please enter a number between {min} and {max}.");
            }
        }
    }
}
