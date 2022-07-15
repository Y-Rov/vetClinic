using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.AnimalViewModel
{
    public class AnimalMedCardViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public bool MeetHasOccureding { get; set; }

        public string Disease { get; set; }

        public int AnimalId { get; set; }
    }
}
