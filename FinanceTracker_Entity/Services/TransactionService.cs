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
    public class TransactionService
    {
        private ConsoleHelper _consHlp;
        private DataContext _context;

        public TransactionService(DataContext context)
        {
            _context = context;
            _consHlp = new ConsoleHelper(context);
        }

        public void AddIncome()
        {
            var user = DataContext.CurrentUser!;
            var account = _consHlp.PickAccount(user.Id);
            if (account == null) return;

            Console.Write("Date (yyyy-MM-dd, blank=today): ");
            var ds = (Console.ReadLine() ?? "").Trim();
            var date = string.IsNullOrWhiteSpace(ds) ? DateTime.Today : DateTime.Parse(ds);

            Console.WriteLine("Choose Category from the list: ");

            var incomeCat = new List<CategoryName>
            { CategoryName.Salary,
              CategoryName.Refund,
              CategoryName.OtherIncome,
              CategoryName.Transfer};

            foreach (var i in incomeCat)
            {
                Console.WriteLine($"{Convert.ToInt32(i)} - {i}");
            }

            CategoryName category = (CategoryName)Enum.Parse(typeof(CategoryName), Console.ReadLine());

            var amount = _consHlp.ReadNonNegativeAmount("Amount: ");

            Console.Write("Description (optional): ");
            var desc = (Console.ReadLine() ?? "").Trim();

            var tx = new Transaction
            {
                //TransactionId = Guid.NewGuid(),
                UserId = user.Id,
                AccountId = account.Id,
                Date = date,
                Type = TransactionType.Income,
                Category = category,
                Amount = amount,
                Currency = account.Currency,
                Description = desc
            };

            _context.Transactions.Add(tx);
            account.Balance += amount;

            _context.SaveChanges();
            //_context.SaveChanges<Account>();
            //_context.SaveChanges<Transaction>();

            Console.WriteLine("Income recorded.");
        }

        public void AddExpense()
        {
            var user = DataContext.CurrentUser!;
            var account = _consHlp.PickAccount(user.Id);
            if (account == null) return;

            Console.Write("Date (yyyy-MM-dd, blank=today): ");
            var ds = (Console.ReadLine() ?? "").Trim();
            var date = string.IsNullOrWhiteSpace(ds) ? DateTime.Today : DateTime.Parse(ds);

            Console.WriteLine("Choose Category from the list: ");

            var expenseCat = new List<CategoryName>
            { CategoryName.Utilities,
              CategoryName.Groceries,
              CategoryName.Transportation,
              CategoryName.Entertainment,
              CategoryName.Health,
              CategoryName.Clothing,
              CategoryName.Insurance,
              CategoryName.Taxes,
              CategoryName.Rent,
              CategoryName.OtherExpense };

            foreach (var e in expenseCat)
            {
                Console.WriteLine($"{Convert.ToInt32(e)} - {e}");
            }


            CategoryName category = (CategoryName)Enum.Parse(typeof(CategoryName), Console.ReadLine());

            var amount = _consHlp.ReadNonNegativeAmount("Amount: ");

            if (amount > account.Balance)
            {
                Console.WriteLine($"Insufficient funds. Balance: {account.Balance:F2} {account.Currency}");
                return;
            }

            Console.Write("Description (optional): ");
            var desc = (Console.ReadLine() ?? "").Trim();

            var tx = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                UserId = user.Id,
                AccountId = account.Id,
                Date = date,
                Type = TransactionType.Expense,
                Category = category,
                Amount = amount,
                Currency = account.Currency,
                Description = desc
            };

            _context.Transactions.Add(tx);
            account.Balance -= amount;

            _context.SaveChanges();
            //_context.SaveChanges<Transaction>();
            //_context.SaveChanges<Account>();

            Console.WriteLine("Expense recorded.");
        }

        public void Transfer()
        {
            var user = DataContext.CurrentUser!;
            Console.WriteLine("\n=== Transfer ===");

            Console.WriteLine("From account:");
            var from = _consHlp.PickAccount(user.Id);
            if (from == null) return;

            Console.WriteLine("To account:");
            var to = _consHlp.PickAccount(user.Id);
            if (to == null) return;

            if (from.Id == to.Id)
            {
                Console.WriteLine("Choose two different accounts.");
                return;
            }


            if (!string.Equals(from.Currency, to.Currency, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Different currencies: {from.Currency} -> {to.Currency}. (Simple version blocks cross-currency.)");
                return;
            }

            var amount = _consHlp.ReadNonNegativeAmount("Amount: ");
            if (amount <= 0m)
            {
                Console.WriteLine("Amount must be > 0.");
                return;
            }
            if (from.Balance < amount)
            {
                Console.WriteLine($"Insufficient funds. Balance: {from.Balance:F2} {from.Currency}");
                return;
            }

            Console.Write("Date (yyyy-MM-dd, blank=today): ");
            var ds = (Console.ReadLine() ?? "").Trim();
            var date = string.IsNullOrWhiteSpace(ds) ? DateTime.Today : DateTime.Parse(ds);

            Console.Write("Description (optional): ");
            var desc = (Console.ReadLine() ?? "").Trim();

            // Create Out transaction
            var outTxId = Guid.NewGuid();
            var outTx = new Transaction
            {
                TransactionId = outTxId,
                UserId = user.Id,
                AccountId = from.Id,
                Date = date,
                Type = TransactionType.TransferOut,
                Category = CategoryName.Transfer,
                Amount = amount,
                Currency = from.Currency,
                Description = desc
            };

            // Create In transaction
            var inTx = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                UserId = user.Id,
                AccountId = to.Id,
                Date = date,
                Type = TransactionType.TransferIn,
                Category = CategoryName.Transfer,
                Amount = amount,
                Currency = to.Currency,
                Description = desc,
                LinkedTransactionId = outTxId
            };

            outTx.LinkedTransactionId = inTx.TransactionId;

            _context.Transactions.Add(outTx);
            _context.Transactions.Add(inTx);

            from.Balance -= amount;
            to.Balance += amount;

            _context.SaveChanges();
            //_context.SaveChanges<Transaction>();
            //_context.SaveChanges<Account>();

            Console.WriteLine("Transfer completed.");
        }

        public void ListSearch()
        {
            var userId = DataContext.CurrentUser!.Id;

            Console.Write("From (yyyy-MM-dd or blank): ");
            var fs = (Console.ReadLine() ?? "").Trim();
            DateTime? from = string.IsNullOrWhiteSpace(fs) ? null : DateTime.Parse(fs);

            Console.Write("To (yyyy-MM-dd or blank): ");
            var ts = (Console.ReadLine() ?? "").Trim();
            DateTime? to = string.IsNullOrWhiteSpace(ts) ? null : DateTime.Parse(ts);

            Console.Write("Account Id or blank for all: ");
            var aidStr = (Console.ReadLine() ?? "").Trim();
            int? accountId = int.TryParse(aidStr, out var gid) ? gid : null;

            Console.Write("Type (Income/Expense/TransferOut/TransferIn) or blank: ");
            var typeStr = (Console.ReadLine() ?? "").Trim();
            TransactionType? type = Enum.TryParse<TransactionType>(typeStr, true, out var parsedT) ? parsedT : null;

            Console.Write("Category (blank=any): ");
            var typeCat = (Console.ReadLine() ?? "").Trim();
            CategoryName? category = Enum.TryParse<CategoryName>(typeCat, true, out var parsedC) ? parsedC : null;

            var rows = _context
                .Transactions
                .Where(t => t.UserId == userId)
                .Where(t => !from.HasValue || t.Date >= from.Value)
                .Where(t => !to.HasValue || t.Date <= to.Value)
                .Where(t => !accountId.HasValue || t.AccountId == accountId.Value)
                .Where(t => !type.HasValue || t.Type == type.Value)
                .Where(t => !category.HasValue || t.Category == category)
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.TransactionId)
                .ToList();

            if (rows.Count == 0)
            {
                Console.WriteLine("\n(No transactions match)");
                return;
            }

            Console.WriteLine("\nId                                   Date        Type          AccountId                            Category         Amount      Cur  Description");
            Console.WriteLine(new string('-', 140));
            foreach (var t in rows)
            {
                Console.WriteLine($"{t.TransactionId}  {t.Date:yyyy-MM-dd}  {t.Type,-12}  {t.AccountId}  {t.Category,-14}  {t.Amount,10:F2}  {t.Currency,-3}  {t.Description}");
            }
        }

        public void DeleteTransaction()
        {
            var userId = DataContext.CurrentUser!.Id;

            Console.Write("Enter Transaction Id to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid id.");
                return;
            }

            var tx = _context.Transactions.FirstOrDefault(t => t.TransactionId == id && t.UserId == userId);
            if (tx == null)
            {
                Console.WriteLine("Transaction not found.");
                return;
            }

            if (tx.Type == TransactionType.TransferOut || tx.Type == TransactionType.TransferIn)
            {
                var pair = _context.Transactions.FirstOrDefault(t => t.TransactionId == tx.LinkedTransactionId);

                Console.Write("This is part of a transfer. Delete both sides? (y/N): ");
                var ans = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
                if (ans != "y") { Console.WriteLine("Cancelled."); return; }

                // Reverse balances
                _consHlp.ReverseBalanceFor(tx);
                if (pair != null) _consHlp.ReverseBalanceFor(pair);

                _context.Transactions.Remove(tx);
                if (pair != null) _context.Transactions.Remove(pair);

                Console.WriteLine("Transfer deleted (both sides).");
                return;
            }

            _consHlp.ReverseBalanceFor(tx);

            _context.Transactions.Remove(tx);
            _context.SaveChanges();
            //_context.SaveChanges<Transaction>();
            //_context.SaveChanges<Account>();

            Console.WriteLine("Transaction deleted.");
        }
    }
}
