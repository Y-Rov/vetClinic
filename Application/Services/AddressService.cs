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
        private readonly IUserService _userService;
        private readonly ILoggerManager _loggerManager;

        public AddressService(
            IAddressRepository addressRepository,
            IUserService userService,
            ILoggerManager loggerManager)
        {
            _addressRepository = addressRepository;
            _userService = userService;
            _loggerManager = loggerManager;
        }

        public async Task<Address> GetAddressByUserIdAsync(int id)
        {
            var address = await _addressRepository.GetAddressByUserIdAsync(id);

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
            var addresses = await _addressRepository.GetAllAddressesAsync();

            _loggerManager.LogInfo("Getting all available addresses");
            return addresses;
        }

        public async Task CreateAddressAsync(Address address)
        {
            var possiblyExistingAddress = await _addressRepository.GetAddressByUserIdAsync(address.UserId);
            
            if (possiblyExistingAddress != null)
            {
                _loggerManager.LogWarn($"User with ID = {address.UserId} has already an address");
                throw new BadRequestException($"User with ID = {address.UserId} has already an address");
            }

            var user = await _userService.GetUserByIdAsync(address.UserId);

            address.User = user;
            await _addressRepository.CreateAddressAsync(address);
            await _addressRepository.SaveChangesAsync();
            _loggerManager.LogInfo($"Address for user with ID = {address.UserId} was created");
        }

        public async Task UpdateAddressAsync(Address address)
        {
            await GetAddressByUserIdAsync(address.UserId);
            
            await _addressRepository.UpdateAddressAsync(address);
            await _addressRepository.SaveChangesAsync();
            _loggerManager.LogInfo($"Address for user with ID = {address.UserId} was updated");
        }

        public async Task DeleteAddressByUserIdAsync(int id)
        {
            var address = await GetAddressByUserIdAsync(id);

            await _addressRepository.DeleteAddressAsync(address);
            await _addressRepository.SaveChangesAsync();
            _loggerManager.LogInfo($"Address for user with ID = {id} was deleted");
        }
    }
}
