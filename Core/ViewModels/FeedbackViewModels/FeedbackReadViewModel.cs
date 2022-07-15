using Core.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.FeedbackViewModels
{
    public class FeedbackReadViewModel : FeedbackBaseViewModel
    {
        public UserReadViewModel User { get; set; }
    }
}
