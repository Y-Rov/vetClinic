using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ILoggerManager _loggerManager;

        public AddressService(
            IAddressRepository addressRepository,
            ILoggerManager loggerManager)
        {
            _addressRepository = addressRepository;
            _loggerManager = loggerManager;
        }

        public async Task<Address> GetAddressByUserIdAsync(int id)
        {
            var address = await _addressRepository.GetAddressByUserIdAsync(id);

            if (address == null)
            {
                _loggerManager.LogWarn($"Address with UserID - {id} wasn't found in the database");
                throw new NotFoundException($"User with ID - {id} doesn't have an address");
            }

            _loggerManager.LogInfo($"Address with UserId - {id} was found");
            return address;
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            var addresses = await _addressRepository.GetAllAddressesAsync();

            _loggerManager.LogInfo("Getting all available addresses...");
            return addresses;
        }

        public async Task CreateAddressAsync(Address address)
        {
            var possibleAddressInTable = await _addressRepository.GetAddressByUserIdAsync(address.UserId);
            
            if (possibleAddressInTable != null)
            {
                _loggerManager.LogWarn($"User with ID - {address.UserId} has already an address!");
                throw new BadRequestException($"User with ID - {address.UserId} has already an address!");
            }

            _loggerManager.LogInfo($"Creating address for user with ID - {address.UserId}...");
            await _addressRepository.CreateAddressAsync(address);
            await _addressRepository.SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(Address address)
        {
            var addressInTable = await _addressRepository.GetAddressByUserIdAsync(address.UserId);
            
            if (addressInTable == null)
            {
                _loggerManager.LogWarn($"User with ID - {address.UserId} doesn't have an address!");
                throw new NotFoundException($"User with ID - {address.UserId} doesn't have an address!");
            }

            _loggerManager.LogInfo($"Updating address for user with ID - {address.UserId}...");
            await _addressRepository.UpdateAddressAsync(address);
            await _addressRepository.SaveChangesAsync();
        }

        public async Task DeleteAddressByUserIdAsync(int id)
        {
            var addressInTable = await _addressRepository.GetAddressByUserIdAsync(id);

            if (addressInTable == null)
            {
                _loggerManager.LogWarn($"User with ID - {id} doesn't have an address!");
                throw new NotFoundException($"User with ID - {id} doesn't have an address!");
            }

            _loggerManager.LogInfo($"Deleting address for user with ID - {id}...");
            await _addressRepository.DeleteAddressAsync(addressInTable);
            await _addressRepository.SaveChangesAsync();
        }
    }
}
