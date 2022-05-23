using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository) => _addressRepository = addressRepository;

        public async Task<Address> GetAddressByIdAsync(int id)
        {
            return await _addressRepository.GetAddressByIdAsync(id);
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            return await _addressRepository.GetAllAddressesAsync();
        }

        public async Task CreateAddressAsync(Address portfolio)
        {
            await _addressRepository.CreateAddressAsync(portfolio);
        }

        public Task<Address> UpdateAddressAsync(Address portfolio)
        {
            return _addressRepository.UpdateAddressAsync(portfolio);
        }

        public async Task DeleteAddressAsync(int id)
        {
            await _addressRepository.DeleteAddressAsync(id);
        }
    }
}
