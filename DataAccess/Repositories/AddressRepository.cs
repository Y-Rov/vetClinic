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
            var address = await _clinicContext.Addresses
                .AsNoTracking()
                .FirstOrDefaultAsync(address => address.UserId == id);

            return address;
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            var addresses = await _clinicContext.Addresses
                .AsNoTracking()
                .ToListAsync();

            return addresses;
        }

        public async Task CreateAddressAsync(Address address)
        {
            await _clinicContext.Addresses.AddAsync(address);
        }

        public async Task UpdateAddressAsync(Address address)
        {
            _clinicContext.Addresses.Update(address);
            await Task.CompletedTask;
        }

        public async Task DeleteAddressAsync(Address address)
        {
            _clinicContext.Addresses.Remove(address);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _clinicContext.SaveChangesAsync();
        }
    }
}
