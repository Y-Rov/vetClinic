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
            var address = await _addressRepository.GetById(id);

            if (address == null)
            {
                _loggerManager.LogWarn($"Address with UserID = {id} doesn't exist");
                throw new NotFoundException($"Address with UserID = {id} wasn't found");
            }

            _loggerManager.LogInfo($"Address with UserID = {id} was found");
            return address;
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            var addresses = await _addressRepository.GetAsync();

            _loggerManager.LogInfo("Getting all available addresses");
            return addresses;
        }

        public async Task CreateAddressAsync(Address newAddress)
        {
            var possiblyExistingAddress = await _addressRepository.GetById(newAddress.UserId);
            
            if (possiblyExistingAddress != null)
            {
                _loggerManager.LogWarn($"User with ID = {newAddress.UserId} has already an address");
                throw new BadRequestException($"User with ID = {newAddress.UserId} has already an address");
            }

            await _addressRepository.InsertAsync(newAddress);
            await _addressRepository.SaveChangesAsync();
            _loggerManager.LogInfo($"Address for user with ID = {newAddress.UserId} was created");
        }

        public async Task UpdateAddressAsync(Address updatedAddress)
        {
            //var address = await GetAddressByUserIdAsync(updatedAddress.UserId);

            //address.City = updatedAddress.City;
            //address.Street = updatedAddress.Street;
            //address.House = updatedAddress.House;
            //address.ApartmentNumber = updatedAddress.ApartmentNumber;
            //address.ZipCode = updatedAddress.ZipCode;

            _addressRepository.Update(updatedAddress);
            await _addressRepository.SaveChangesAsync();
            _loggerManager.LogInfo($"Address for user with ID = {updatedAddress.UserId} was updated");
        }

        public async Task DeleteAddressByUserIdAsync(int id)
        {
            var address = await GetAddressByUserIdAsync(id);

            _addressRepository.Delete(address);
            await _addressRepository.SaveChangesAsync();
            _loggerManager.LogInfo($"Address for user with ID = {id} was deleted");
        }
    }
}
