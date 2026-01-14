using FinanceTracker_Entity.Data;

namespace FinanceTracker_Entity
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataContext _context = new DataContext();




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

        }
    }
}
