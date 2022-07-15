using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.FeedbackViewModels
{
    public class FeedbackBaseViewModel
    {
        public string Email { get; set; }
        public int ServiceRate { get; set; }
        public int PriceRate { get; set; }
        public int SupportRate { get; set; }
        public string? Suggestions { get; set; }
    }
}
