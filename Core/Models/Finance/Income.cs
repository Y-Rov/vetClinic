using Core.Entities;

namespace Core.Models.Finance
{
    public class Income
    {
        public int AppointmenId { get; set; }
        public IEnumerable<Procedure> ListOfProcedures { get; set; } = new List<Procedure>();
        public decimal Cost { get; set; }
    }
}
