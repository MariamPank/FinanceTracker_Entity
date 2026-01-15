using FinanceTracker_Entity.Data;
using FinanceTracker_Entity.Models;
using FinanceTracker_Entity.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinanceTracker_Entity.Models.Enums;

namespace FinanceTracker_Entity.Services
{
    internal class BudgetService
    {
        private DataContext _context;
        ConsoleHelper _consHlp;

        public BudgetService(DataContext context)
        {
            _context = context;
            _consHlp = new ConsoleHelper(context);
        }

        public void SetOrUpdateBudget()
        {
            var user = DataContext.CurrentUser!;

            Console.WriteLine("\n=== Set / Update Monthly Budget ===");

            Console.WriteLine("Choose Category from the list: ");
            var categoryList = Enum.GetValues(typeof(CategoryName));

            foreach (var c in categoryList)
            {
                Console.WriteLine($"{Convert.ToInt32(c)} - {c}");
            }

            var category = (CategoryName)Enum.Parse(typeof(CategoryName), _consHlp.ReadNonEmpty("Category: "));

            int year = _consHlp.ReadIntInRange("Year (e.g., 2025): ", 1900, 3000);
            int month = _consHlp.ReadIntInRange("Month (1-12): ", 1, 12);

            Console.Write($"Currency (default GEL): ");
            var ccy = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(ccy)) ccy = "GEL";

            decimal amount = _consHlp.ReadNonNegativeAmount("Monthly Amount: ");

            var existing = _context.Budgets.FirstOrDefault(b =>
                b.UserId == user.Id &&
                b.Year == year &&
                b.Month == month &&
                b.Category.Equals(category));

            if (existing == null)
            {
                var newBudget = new Budget
                {
                    BudgetId = _context.Budgets.Count == 0 ? 1 : _context.Budgets.Max(b => b.BudgetId) + 1,
                    UserId = user.Id,
                    Category = category,
                    Year = year,
                    Month = month,
                    Amount = amount,
                    Currency = ccy
                };
                _context.Budgets.Add(newBudget);
                Console.WriteLine("Budget created.");
            }
            else
            {
                existing.Amount = amount;
                existing.Currency = ccy;
                Console.WriteLine("Budget updated.");
            }

            _context.SaveChanges();
        }
        public void RemoveBudget()
        {
            var userID = DataContext.CurrentUser!.Id;
            Console.WriteLine("\n=== Remove Budget ===");

            Console.Write("Category: ");
            var typeCat = (Console.ReadLine() ?? "").Trim();
            CategoryName? category = Enum.TryParse<CategoryName>(typeCat, true, out var parsedC) ? parsedC : null;
            int year = _consHlp.ReadIntInRange("Year: ", 1900, 3000);
            int month = _consHlp.ReadIntInRange("Month (1-12): ", 1, 12);

            var budget = _context.Budgets.FirstOrDefault(b =>
                b.UserId == userID &&
                b.Year == year &&
                b.Month == month &&
                b.Category.Equals(category));

            if (budget == null)
            {
                Console.WriteLine("Not found.");
                return;
            }

            Console.Write($"Confirm delete {category} {year}-{month:00}? (y/N): ");
            if ((Console.ReadLine() ?? "").Trim().ToLowerInvariant() == "y")
            {
                _context.Budgets.Remove(budget);
                _context.SaveChanges();
                Console.WriteLine("Deleted.");
            }
            else
            {
                Console.WriteLine("Cancelled.");
            }
        }
        public void ListBudgets()
        {
            var user = DataContext.CurrentUser!;
            Console.Write("\nFilter by Year (blank = all): ");
            var ys = (Console.ReadLine() ?? "").Trim();
            int? year = int.TryParse(ys, out var yParsed) ? yParsed : null;

            Console.Write("Filter by Month 1-12 (blank = all): ");
            var ms = (Console.ReadLine() ?? "").Trim();
            int? month = int.TryParse(ms, out var mParsed) ? mParsed : null;

            var rows = _context.Budgets
                .Where(b => b.UserId == user.Id)
                .Where(b => !year.HasValue || b.Year == year.Value)
                .Where(b => !month.HasValue || b.Month == month.Value)
                .OrderBy(b => b.Year).ThenBy(b => b.Month).ThenBy(b => b.Category)
                .ToList();

            if (rows.Count == 0)
            {
                Console.WriteLine("\n(No budgets)");
                return;
            }

            Console.WriteLine("\nYear  Month  Category           Amount       Cur");
            Console.WriteLine(new string('-', 55));
            foreach (var b in rows)
            {
                Console.WriteLine($"{b.Year,-5} {b.Month,5}  {b.Category,-18}  {b.Amount,10:F2}   {b.Currency}");
            }
        }
        public void BudgetVsActual()
        {
            var user = DataContext.CurrentUser!;
            Console.WriteLine("\n=== Budget vs Actual (Expenses) ===");
            int year = _consHlp.ReadIntInRange("Year: ", 1900, 3000);
            int month = _consHlp.ReadIntInRange("Month (1-12): ", 1, 12);

            var actuals = _context.Transactions
                .Where(t => t.UserId == user.Id &&
                            t.Date.Year == year &&
                            t.Date.Month == month &&
                            (t.Type == TransactionType.Expense))
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

            var budgets = _context.Budgets
                .Where(b => b.UserId == user.Id && b.Year == year && b.Month == month)
                .ToList();

            var categories = budgets.Select(b => b.Category)
                                    .Union(actuals.Keys)
                                    .OrderBy(c => c)
                                    .ToList();

            if (categories.Count == 0)
            {
                Console.WriteLine("(No budgets or expenses for this month)");
                return;
            }

            Console.WriteLine("\nCategory           Budget      Actual      Variance");
            Console.WriteLine(new string('-', 60));
            foreach (var cat in categories)
            {
                var budget = budgets.FirstOrDefault(b => b.Category.Equals(cat));
                var bAmt = budget?.Amount ?? 0m;
                var aAmt = actuals.TryGetValue(cat, out var act) ? act : 0m;
                var variance = bAmt - aAmt;
                Console.WriteLine($"{cat,-18} {bAmt,10:F2}  {aAmt,10:F2}  {variance,10:F2}");
            }
        }
    }
}
