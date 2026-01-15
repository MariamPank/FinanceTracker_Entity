using FinanceTracker_Entity.Data;
using FinanceTracker_Entity.Models;
using FinanceTracker_Entity.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker_Entity.Services
{
    public class AuthService
    {
        private DataContext _context;
        UserValidator _validator = new UserValidator();


        public AuthService(DataContext context)
        {
            _context = context;
        }

        public void Register()
        {
            Console.WriteLine("Register");
            Console.Write("Enter your username: ");
            string usename = Console.ReadLine();

            Console.Write("Enter your email: ");
            string email = Console.ReadLine();

            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            var existingUser = _context.Users.FirstOrDefault(e => e.Email == email);

            if (existingUser != null) throw new Exception("User already exists!");

            User newUser = new User()
            {
                UserName = usename,
                Email = email,
                Password = password
            };

            var result = _validator.Validate(newUser);

            if (result.IsValid)
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
        }

        public void Login()
        {
            Console.WriteLine("Log in");
            Console.Write("Enter your email: ");
            string LoginEmail = Console.ReadLine();

            Console.Write("Enter your password: ");
            string LoginPassword = Console.ReadLine();

            var LoggedInUser = _context.Users.FirstOrDefault(e => e.Email == LoginEmail && e.Password == LoginPassword);

            if (LoggedInUser == null)
                throw new Exception("Invalid email or password!");

            DataContext.CurrentUser = LoggedInUser;
            Console.WriteLine($"Welcome, {LoggedInUser.UserName}");
            Program.Show();
        }

    }
}
