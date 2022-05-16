using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Animal
    {
        public int Id { get; set; }
        public string? NickName { get; set; }
        public DateTime BirthDate { get; set; }
        public int UserId { get; set; }
    }
}
