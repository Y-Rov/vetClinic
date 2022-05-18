using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Salary
    {
        public int UserId { get; set; }
        public User EmployeeUser { get; set; }
        public decimal Value { get; set; }
    }
}
