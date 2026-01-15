using FinanceTracker_Entity.Data;
using FinanceTracker_Entity.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinanceTracker_Entity.Models.Enums;

namespace FinanceTracker_Entity.Services
{
    internal class ReportService
    {
        private DataContext _context;
        ConsoleHelper _consHlp;

        public ReportService(DataContext context)
        {
            _context = context;
            _consHlp = new ConsoleHelper(context);
        }

        public void MonthlySummary()
        {
            var userId = DataContext.CurrentUser!.Id;
            int year = _consHlp.ReadIntInRange("Year: ", 1900, 3000);
            int month = _consHlp.ReadIntInRange("Month (1-12): ", 1, 12);

            var monthRows = _context.Transactions
                .Where(t => t.UserId == userId &&
                            t.Date.Year == year &&
                            t.Date.Month == month)
                .ToList();

            var income = monthRows.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var expense = monthRows.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
            var net = income - expense;

            Console.WriteLine($"\n=== {year}-{month:00} Summary ===");
            Console.WriteLine($"Income : {income,12:F2}");
            Console.WriteLine($"Expense: {expense,12:F2}");
            Console.WriteLine($"Net    : {net,12:F2}");

            Console.WriteLine("\nBy Category (Expenses):");
            var byCat = monthRows
                .Where(t => t.Type == TransactionType.Expense)
                .GroupBy(t => t.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) })
                .OrderByDescending(x => x.Total)
                .ToList();

            if (byCat.Count == 0) { Console.WriteLine("(No expenses)"); }
            else
            {
                Console.WriteLine("Category           Total");
                Console.WriteLine(new string('-', 32));
                foreach (var r in byCat)
                    Console.WriteLine($"{r.Category,-18} {r.Total,10:F2}");
            }
        }

        public void YtdSummary()
        {
            var userId = DataContext.CurrentUser!.Id;
            int year = _consHlp.ReadIntInRange("Year: ", 1900, 3000);

            var rows = _context.Transactions
                .Where(t => t.UserId == userId && t.Date.Year == year)
                .ToList();

            var income = rows.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var expense = rows.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
            var net = income - expense;

            Console.WriteLine($"\n=== YTD {year} ===");
            Console.WriteLine($"Income : {income,12:F2}");
            Console.WriteLine($"Expense: {expense,12:F2}");
            Console.WriteLine($"Net    : {net,12:F2}");

            Console.WriteLine("\nMonthly Net:");
            Console.WriteLine("Month      Income       Expense       Net");
            Console.WriteLine(new string('-', 46));

            for (int m = 1; m <= 12; m++)
            {
                var mRows = rows.Where(t => t.Date.Month == m).ToList();
                var inc = mRows.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
                var exp = mRows.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
                Console.WriteLine($"{CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(m),-5} {inc,12:F2} {exp,12:F2} {(inc - exp),10:F2}");
            }
        }

        public void AccountBalances()
        {
            var userId = DataContext.CurrentUser!.Id;

            var rows = _context.Accounts
                .Where(a => a.UserId == userId && a.IsActive)
                .OrderBy(a => a.Name)
                .ToList();

            if (rows.Count == 0)
            {
                Console.WriteLine("\n(No accounts)");
                return;
            }

            Console.WriteLine("\nId                                   Name                 Type     Currency   Balance");
            Console.WriteLine(new string('-', 90));
            foreach (var a in rows)
                Console.WriteLine($"{a.Id}  {a.Name,-20} {a.Type,-7}  {a.Currency,-8}  {a.Balance,10:F2}");
        }

        public void TopExpenseCategories()
        {
            var userId = DataContext.CurrentUser!.Id;
            int year = _consHlp.ReadIntInRange("Year: ", 1900, 3000);
            int month = _consHlp.ReadIntInRange("Month (1-12): ", 1, 12);
            int n = _consHlp.ReadIntInRange("Top N (e.g., 5): ", 1, 50);

            var rows = _context.Transactions
                .Where(t => t.UserId == userId &&
                            t.Date.Year == year &&
                            t.Date.Month == month &&
                            t.Type == TransactionType.Expense)
                .GroupBy(t => t.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) })
                .OrderByDescending(x => x.Total)
                .Take(n)
                .ToList();

            if (rows.Count == 0)
            {
                Console.WriteLine("\n(No expenses for that month)");
                return;
            }

            Console.WriteLine("\nCategory           Total");
            Console.WriteLine(new string('-', 32));
            foreach (var r in rows)
                Console.WriteLine($"{r.Category,-18} {r.Total,10:F2}");
        }
    }
}
