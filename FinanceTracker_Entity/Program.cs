using FinanceTracker_Entity.Data;
using FinanceTracker_Entity.Menus;
using FinanceTracker_Entity.Services;

namespace FinanceTracker_Entity
{
    internal class Program
    {
        private static DataContext _context = new DataContext();

        private static AccountService _accountService = new AccountService(_context);
        private static UserService _userService = new UserService(_context);
        private static TransactionService _transactionService = new TransactionService(_context);
        private static BudgetService _budgetService = new BudgetService(_context);
        private static AuthService _authService = new AuthService(_context);
        private static ReportService _reportService = new ReportService(_context);

        private static AuthMenu _authMenu = new AuthMenu(_authService);
        private static AccountMenu _accountMenu = new AccountMenu(_accountService);
        private static TransactionMenu _transactionMenu = new TransactionMenu(_transactionService);
        private static ReportMenu _reportMenu = new ReportMenu(_reportService);
        private static BudgetMenu _budgetMenu = new BudgetMenu(_budgetService);
        private static UserMenu _userMenu = new UserMenu(_accountMenu, _userService, _transactionMenu, _reportMenu, _budgetMenu);


        //using Microsoft.EntityFrameworkCore;
        //using FinanceTracker_FinalProject.Data;

        //var builder = new DbContextOptionsBuilder<AppDbContext>();
        //builder.UseSqlServer("Server=.;Database=FinanceTrackerDb;Trusted_Connection=True;");

        //var db = new AppDbContext(builder.Options);

        //AccountService accountService = new(db);
        //UserService userService = new(db);
        //TransactionService transactionService = new(db);
        //BudgetService budgetService = new(db);
        //AuthService authService = new(db);

        static void Main(string[] args)
        {

            Show();
        }

        public static void Show()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader();
                if (DataContext.CurrentUser == null)
                {
                    _authMenu.Show();
                }
                else
                {
                    _userMenu.Show();
                }
                break;
            }
        }

        private static void PrintHeader()
        {
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("           FINANCE TRACKER");
            Console.WriteLine("═══════════════════════════════════════");

            if (DataContext.CurrentUser == null)
            {
                Console.WriteLine("Status: Not logged in\n");
            }
            else
            {
                var user = DataContext.CurrentUser;
                Console.WriteLine($"User: {user.UserName}   |   Email: {user.Email}\n");
            }
        }
    }
}
