using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinanceTracker_Entity.Models.Enums;

namespace FinanceTracker_Entity.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "GEL";


        public CategoryName Category { get; set; } // which relationship???


        public int UserId { get; set; }
        public User User { get; set; }
    }

}
