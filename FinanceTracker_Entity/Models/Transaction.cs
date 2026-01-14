using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinanceTracker_Entity.Models.Enums;

namespace FinanceTracker_Entity.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public CategoryName Category { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "GEL";
        public string Description { get; set; } = "";
        //public Guid LinkedTransactionId { get; set; } // Transfer pair

        // many -> one
        public int UserId { get; set; }
        public User User { get; set; }
        public int AccountId { get; set; }

        // many-to-many
        // public List<TransactionCategory> TransactionCategories { get; set; } = new();

    }

}
