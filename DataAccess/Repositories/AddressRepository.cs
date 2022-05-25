using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ClinicContext _clinicContext;

        public AddressRepository(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }

        public async Task<Address?> GetAddressByUserIdAsync(int id)
        {
            return await _clinicContext.Addresses.FirstOrDefaultAsync(address => address.UserId == id);
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            return await _clinicContext.Addresses.ToListAsync();
        }

        public async Task CreateAddressAsync(Address address)
        {
            await _clinicContext.Addresses.AddAsync(address);
        }

        public async Task UpdateAddressAsync(Address address)
        {
            _clinicContext.Addresses.Update(address);
        }

        public async Task DeleteAddressByUserIdAsync(int id)
        {
            _clinicContext.Remove(GetAddressByUserIdAsync(id));
        }

        public async Task SaveChangesAsync()
        {
            await _clinicContext.SaveChangesAsync();
        }
    }
}
