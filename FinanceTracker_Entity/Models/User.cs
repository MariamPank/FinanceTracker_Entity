using FinanceTracker_Entity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinanceTracker_Entity.Models.Enums;

namespace FinanceTracker_Entity.Models
{
    public class User : BaseEntity
    {
        //public int Id { get; set; } 
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

        // 1-1
        public UserProfile Profile { get; set; }

        // 1-many
        public List<Transaction> Transactions { get; set; } = new();
        public List<Account> Accounts { get; set; } = new();
    }
}
