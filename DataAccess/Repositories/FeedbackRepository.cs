using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }
    }
}
