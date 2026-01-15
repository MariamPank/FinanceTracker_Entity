using FinanceTracker_Entity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker_Entity.Menus
{
    internal class BudgetMenu
    {
        private BudgetService _budgetService;

        public BudgetMenu(BudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        public void Show()
        {
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("\n═══════════════════════════════════════");
                Console.WriteLine("            BUDGETS MENU");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("1) Set / Update Category Budget");
                Console.WriteLine("2) Remove Budget");
                Console.WriteLine("3) List Budgets");
                Console.WriteLine("4) Budget vs Actual (by Month)");
                Console.WriteLine("0) Back");
                Console.Write("Choose: ");

                switch ((Console.ReadLine() ?? "").Trim())
                {
                    case "1": _budgetService.SetOrUpdateBudget(); break;
                    case "2": _budgetService.RemoveBudget(); break;
                    case "3": _budgetService.ListBudgets(); break;
                    case "4": _budgetService.BudgetVsActual(); break;
                    case "0": loop = false; break;
                    default: Console.WriteLine("Unknown option."); break;
                }
            }
        }
    }
}
