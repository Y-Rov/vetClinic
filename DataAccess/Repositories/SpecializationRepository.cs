using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SpecializationRepository : Repository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(ClinicContext context) : base(context)
        {

        }
    }
}
