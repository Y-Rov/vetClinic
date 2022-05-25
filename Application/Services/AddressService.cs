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

        public AddressService(IAddressRepository addressRepository, ILoggerManager loggerManager)
        {
            _addressRepository = addressRepository;
            _loggerManager = loggerManager;
        }

        public async Task<Address> GetAddressByUserIdAsync(int id)
        {
            var address = await _addressRepository.GetAddressByUserIdAsync(id);
            if (address != null)
            {
                return address;
            }

            _loggerManager.LogWarn($"User with ID - {id} doesn't have an address!");
            throw new NotFoundException($"User with ID - {id} doesn't have an address!");
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            return await _addressRepository.GetAllAddressesAsync();
        }

        public async Task CreateAddressAsync(Address address)
        {
            var possibleAddressInTable = await _addressRepository.GetAddressByUserIdAsync(address.UserId);
            if (possibleAddressInTable == null)
            {
                await _addressRepository.CreateAddressAsync(address);
                await _addressRepository.SaveChangesAsync();
            }

            _loggerManager.LogWarn($"User with ID - {address.UserId} has already an address!");
            throw new BadRequestException($"User with ID - {address.UserId} has already an address!");
        }

        public async Task UpdateAddressAsync(Address address)
        {
            var portfolioInTable = await _addressRepository.GetAddressByUserIdAsync(address.UserId);
            if (portfolioInTable != null)
            {
                await _addressRepository.UpdateAddressAsync(address);
                await _addressRepository.SaveChangesAsync();
            }

            _loggerManager.LogWarn($"User with ID - {address.UserId} doesn't have an address!");
            throw new NotFoundException($"User with ID - {address.UserId} doesn't have an address!");
        }

        public async Task DeleteAddressByUserIdAsync(int id)
        {
            var portfolioInTable = await _addressRepository.GetAddressByUserIdAsync(id);
            if (portfolioInTable != null)
            {
                await _addressRepository.DeleteAddressByUserIdAsync(id);
                await _addressRepository.SaveChangesAsync();
            }

            _loggerManager.LogWarn($"User with ID - {id} doesn't have an address!");
            throw new NotFoundException($"User with ID - {id} doesn't have an address!");
        }
    }
}
