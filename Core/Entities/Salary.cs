using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    //Class Salary contains info about employees salary
    public class Salary
    {
        public int UserId { get; set; }
        
        //public User User_obj { get; set; }  //do we need this?

        public decimal Wage { get; set; }

    }
}
