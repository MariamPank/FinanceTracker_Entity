using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinanceTracker_Entity.Models.Enums;

namespace FinanceTracker_Entity.Models
{
    public class TransactionCategory
    {
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }

        public int CategoryId { get; set; }
        //public Category Category { get; set; } //????
    }

}
