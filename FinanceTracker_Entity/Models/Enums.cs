using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker_Entity.Models
{
    public class Enums
    {
        public enum UserRole { User = 0, Admin = 1 }
        public enum AccountType { Cash = 0, Bank = 1, Card = 2 }
        public enum TransactionType { Income = 0, Expense = 1, TransferOut = 2, TransferIn = 3 }

        public enum CategoryName
        {
            Salary = 0,
            Refund = 1,
            OtherIncome = 2,
            Transfer = 3,

            Utilities = 4,
            Groceries = 5,
            Transportation = 6,
            Entertainment = 7,
            Health = 8,
            Clothing = 9,
            Insurance = 10,
            Taxes = 11,
            Rent = 12,
            OtherExpense = 13
        }
    }
}
