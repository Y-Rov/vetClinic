using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ILoggerManager _loggerManager;

        public AddressService(IAddressRepository addressRepository, ILoggerManager loggerManager)
        {
            _addressRepository = addressRepository;
            _loggerManager = loggerManager;
        }

        public async Task<Address> GetAddressByUserIdAsync(int id)
        {
            _loggerManager.LogInfo("Inside GetAddressByUserIdAsync");
            var address = await _addressRepository.GetAddressByUserIdAsync(id);
            if (address != null)
            {
                _loggerManager.LogInfo("Return from GetAddressByUserIdAsync");
                return address;
            }

            _loggerManager.LogError($"User with ID - {id} doesn't have an address!");
            throw new NullReferenceException();
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            return await _addressRepository.GetAllAddressesAsync();
        }

        public async Task CreateAddressAsync(Address address)
        {
            await _addressRepository.CreateAddressAsync(address);
        }

        public async Task UpdateAddressAsync(Address address)
        {
            await _addressRepository.UpdateAddressAsync(address);
        }

        public async Task DeleteAddressByUserIdAsync(int id)
        {
            await _addressRepository.DeleteAddressByUserIdAsync(id);
        }
    }
}
