using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.SpecializationViewModels
{
    public class SpecializationUpdateViewModel : SpecializationBaseViewModel
    {
        public IEnumerable<int>? ProcedureIds { get; set; }
        public IEnumerable<int>? UsersIds { get; set; }
    }
}
