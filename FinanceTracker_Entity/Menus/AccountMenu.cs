using FinanceTracker_Entity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker_Entity.Menus
{
    public class AccountMenu
    {
        private AccountService _accountService;

        public AccountMenu(AccountService accountService)
        {
            _accountService = accountService;
        }

        public void Show()
        {
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("\n═══════════════════════════════════════");
                Console.WriteLine("           ACCOUNTS MENU");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("1) Create Account");
                Console.WriteLine("2) List Accounts");
                Console.WriteLine("3) Rename Account");
                Console.WriteLine("4) Delete Account");
                Console.WriteLine("0) Back");
                Console.Write("Choose: ");

                switch ((Console.ReadLine() ?? "").Trim())
                {
                    case "1": _accountService.CreateAccount(); break;
                    case "2": _accountService.ListAccounts(); break;
                    case "3": _accountService.RenameAccount(); break;
                    case "4": _accountService.DeleteAccount(); break;
                    case "0": loop = false; break;
                    default: Console.WriteLine("Unknown option."); break;
                }
            }
        }
    }
}
