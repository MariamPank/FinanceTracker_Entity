using FinanceTracker_Entity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker_Entity.Menus
{
    internal class UserMenu
    {
        private AccountMenu _accountMenu;
        private UserService _userService;
        private TransactionMenu _transactionMenu;
        private ReportMenu _reportMenu;
        private BudgetMenu _budgetMenu;

        public UserMenu(AccountMenu accountMenu, UserService userService, TransactionMenu transactionMenu, ReportMenu reportMenu, BudgetMenu budgetMenu)
        {
            _accountMenu = accountMenu;
            _userService = userService;
            _transactionMenu = transactionMenu;
            _reportMenu = reportMenu;
            _budgetMenu = budgetMenu;
        }

        public void Show()
        {
            Console.WriteLine("\n═══════════════════════════════════════");
            Console.WriteLine("               USER MENU");
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("1) Transactions");
            Console.WriteLine("2) Accounts");
            Console.WriteLine("3) Reports");
            Console.WriteLine("4) Budgets");
            Console.WriteLine("5) My Profile");
            Console.WriteLine("6) Change Password");
            Console.WriteLine("7) Logout");
            Console.WriteLine("0) Exit");
            Console.Write("Choose: ");
            var choice = (Console.ReadLine() ?? "").Trim();

            switch (choice)
            {
                case "1": _transactionMenu.Show(); Show(); break;
                case "2": _accountMenu.Show(); Show(); break;
                case "3": _reportMenu.Show(); Show(); break;
                case "4": _budgetMenu.Show(); Show(); break;
                case "5": _userService.ViewProfile(); Show(); break;
                case "6": _userService.ChangePassword(); Show(); break;
                case "7": _userService.Logout(); Show(); break;
                case "0": break;
                default: Console.WriteLine("Unknown option."); Show(); break;
            }
        }
    }
}
