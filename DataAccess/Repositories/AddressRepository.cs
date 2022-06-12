using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(ClinicContext clinicContext) : base(clinicContext)
        {
            
        }
    }
}
