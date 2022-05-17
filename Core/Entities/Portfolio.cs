using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Portfolio
    {
        public string? Description { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
