using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.AnimalViewModel
{
    public class AnimalViewModel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string? NickName { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
