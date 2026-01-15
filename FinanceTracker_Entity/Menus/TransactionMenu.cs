using FinanceTracker_Entity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker_Entity.Menus
{
    internal class TransactionMenu
    {
        private TransactionService _transactionService;

        public TransactionMenu(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public void Show()
        {
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("\n═══════════════════════════════════════");
                Console.WriteLine("          TRANSACTIONS MENU");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("1) Add Income");
                Console.WriteLine("2) Add Expense");
                Console.WriteLine("3) Transfer");
                Console.WriteLine("4) List / Search");
                Console.WriteLine("5) Delete Transaction");
                Console.WriteLine("0) Back");
                Console.Write("Choose: ");

                switch ((Console.ReadLine() ?? "").Trim())
                {
                    case "1": _transactionService.AddIncome(); break;
                    case "2": _transactionService.AddExpense(); break;
                    case "3": _transactionService.Transfer(); break;
                    case "4": _transactionService.ListSearch(); break;
                    case "5": _transactionService.DeleteTransaction(); break;
                    case "0":
                        loop = false;
                        break;
                    default: Console.WriteLine("Unknown option."); break;
                }
            }
        }
    }
}
