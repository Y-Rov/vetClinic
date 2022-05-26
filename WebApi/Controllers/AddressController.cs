using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.AddressViewModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IViewModelMapper<Address, AddressViewModel> _addressViewModelBaseMapper;
        private readonly IViewModelMapper<AddressViewModel, Address> _addressMapper;

        public AddressController(IAddressService addressService,
            IViewModelMapper<Address, AddressViewModel> addressViewModelBaseMapper,
            IViewModelMapper<AddressViewModel, Address> addressMapper)
        {
            _addressService = addressService;
            _addressViewModelBaseMapper = addressViewModelBaseMapper;
            _addressMapper = addressMapper;
        }

        [HttpGet("/api/addresses/")]
        public async Task<ActionResult<IEnumerable<AddressViewModel>>> GetAsync()
        {
            var addresses = await _addressService.GetAllAddressesAsync();

            var viewModels = addresses.Select(p => _addressViewModelBaseMapper.Map(p)).ToList();
            return Ok(viewModels);
        }

        [HttpGet("/api/addresses/{id:int:min(1)}")]
        public async Task<ActionResult<AddressViewModel>> GetAsync([FromRoute] int id)
        {
            var address = await _addressService.GetAddressByUserIdAsync(id);

            var viewModel = _addressViewModelBaseMapper.Map(address);
            return Ok(viewModel);
        }

        [HttpPost("/api/addresses/")]
        public async Task<ActionResult> CreateAsync(AddressViewModel addressWithApartment)
        {
            await _addressService.CreateAddressAsync(_addressMapper.Map(addressWithApartment));
            return NoContent();
        }

        [HttpPatch("/api/addresses/")]
        public async Task<ActionResult> UpdateAsync(AddressViewModel addressWithApartment)
        {
            await _addressService.UpdateAddressAsync(_addressMapper.Map(addressWithApartment));
            return NoContent();
        }

        [HttpDelete("/api/addresses/{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await _addressService.DeleteAddressByUserIdAsync(id);
            return NoContent();
        }
    }
}