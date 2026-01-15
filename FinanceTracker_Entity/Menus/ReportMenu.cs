using FinanceTracker_Entity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker_Entity.Menus
{
    internal class ReportMenu
    {
        private ReportService _reportService;

        public ReportMenu(ReportService reportService)
        {
            _reportService = reportService;
        }

        public void Show()
        {
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("\n═══════════════════════════════════════");
                Console.WriteLine("              REPORTS");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("1) Monthly Summary");
                Console.WriteLine("2) Year-to-Date Summary");
                Console.WriteLine("3) Account Balances");
                Console.WriteLine("4) Top N Expense Categories (Month)");
                Console.WriteLine("0) Back");
                Console.Write("Choose: ");

                switch ((Console.ReadLine() ?? "").Trim())
                {
                    case "1": _reportService.MonthlySummary(); break;
                    case "2": _reportService.YtdSummary(); break;
                    case "3": _reportService.AccountBalances(); break;
                    case "4": _reportService.TopExpenseCategories(); break;
                    case "0": loop = false; break;
                    default: Console.WriteLine("Unknown option."); break;
                }
            }
        }
    }
}
