using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker_Entity.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }

        // 1-1
        public int UserId { get; set; }
        public User User { get; set; }
    }

}
