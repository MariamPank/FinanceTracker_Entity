using FinanceTracker_Entity.Data;
using FinanceTracker_Entity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker_Entity.Menus
{
    public class AuthMenu
    {
        private AuthService _authService;

        public AuthMenu(AuthService authService)
        {
            _authService = authService;
        }

        public void Show()
        {
            while (DataContext.CurrentUser == null)
            {
                Console.WriteLine("\n═══════════════════════════════════════");
                Console.WriteLine("           AUTHORISATION MENU");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("Auth Menu");
                Console.WriteLine("1) Register");
                Console.WriteLine("2) Login");
                Console.Write("Enter your choise: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": Console.Clear(); _authService.Register(); break;
                    case "2": Console.Clear(); _authService.Login(); break;
                    default: Console.WriteLine("Unknown option."); break;
                }
            }
        }
    }
}
