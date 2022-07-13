using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Paginator.Parameters
{
    public class FeedbackParameters
    {
        public string? FilterParam { get; init; }
        public string? OrderByParam { get; set; }
        public string? OrderByDirection { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }
    }
}
