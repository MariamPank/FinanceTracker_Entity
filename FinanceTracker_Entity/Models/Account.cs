using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinanceTracker_Entity.Models.Enums;

namespace FinanceTracker_Entity.Models
{
    public class Account
    {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public AccountType Type { get; set; }
            public string Currency { get; set; } = "GEL";
            public decimal Balance { get; set; } = 0;
            public DateTime CreatedDate { get; set; }
            public bool IsActive { get; set; } = true;

            public int UserId { get; set; }
            public User User { get; set; }

    }
}
