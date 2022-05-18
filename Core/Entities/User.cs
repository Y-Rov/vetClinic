using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime BirthDate { get; set;}

        // Salary
        // Address

        // public IList<Animal> Animals { get; set; }
        // public IList<Appointment> Appointments { get; set; }
    }
}
