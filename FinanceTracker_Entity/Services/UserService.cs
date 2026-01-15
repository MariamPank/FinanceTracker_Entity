using FinanceTracker_Entity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker_Entity.Services
{
    internal class UserService
    {
        private DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public void ViewProfile()
        {
            var u = DataContext.CurrentUser;
            Console.WriteLine("\n=== My Profile ===");
            Console.WriteLine($"UserId      : {u.Id}");
            Console.WriteLine($"Username    : {u.UserName}");
            Console.WriteLine($"Email       : {u.Email}");
            Console.WriteLine($"Role        : {u.Role}");
            Console.WriteLine($"Created     : {u.CreateAt:yyyy-MM-dd}");
            Console.WriteLine("0) Back");
            Console.Write("Choose: ");

            var choice = (Console.ReadLine() ?? "").Trim();
            switch (choice)
            {
                case "0": break;
            }
        }

        public void Logout()
        {
            DataContext.CurrentUser = null;
            Console.WriteLine("You have been logged out");

        }

        public void ChangePassword()
        {
            if (DataContext.CurrentUser == null) throw new Exception("You are not logged in!");
            Console.WriteLine("Change password");

            Console.Write("Enter your email: ");
            string email = Console.ReadLine();

            Console.Write("Enter your old password: ");
            string oldPassword = Console.ReadLine();

            Console.Write("Enter you new password: ");
            string newPassword = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newPassword) && newPassword?.Length < 8)
            {
                throw new Exception("New password cannot be empty!");
            }

            if (DataContext.CurrentUser.Password != oldPassword)
            {
                throw new Exception("Old passwords do not match");
            }

            var user = _context.Users.FirstOrDefault(e => e.Id == DataContext.CurrentUser.Id);
            if (user == null) throw new Exception("User not found!");

            user.Password = newPassword;
            _context.SaveChanges();
        }
    }
}
