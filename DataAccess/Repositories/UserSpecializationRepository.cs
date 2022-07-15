using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories
{
    public class UserSpecializationRepository : Repository<UserSpecialization>, IUserSpecializationRepository
    {
        public UserSpecializationRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }
    }
}
