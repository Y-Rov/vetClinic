using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Specialization
    {
        public int SpecializationId { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
