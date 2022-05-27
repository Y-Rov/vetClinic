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

        public AddressController(
            IAddressService addressService,
            IViewModelMapper<Address, AddressViewModel> addressViewModelBaseMapper,
            IViewModelMapper<AddressViewModel, Address> addressMapper)
        {
            _addressService = addressService;
            _addressViewModelBaseMapper = addressViewModelBaseMapper;
            _addressMapper = addressMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressViewModel>>> GetAsync()
        {
            var addresses = await _addressService.GetAllAddressesAsync();

            var viewModels = addresses.Select(p => _addressViewModelBaseMapper.Map(p));
            return Ok(viewModels);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<AddressViewModel>> GetAsync([FromRoute] int id)
        {
            var address = await _addressService.GetAddressByUserIdAsync(id);

            var viewModel = _addressViewModelBaseMapper.Map(address);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(AddressViewModel addressWithApartment)
        {
            var address = _addressMapper.Map(addressWithApartment);

            await _addressService.CreateAddressAsync(address);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(AddressViewModel addressWithApartment)
        {
            var address = _addressMapper.Map(addressWithApartment);

            await _addressService.UpdateAddressAsync(address);
            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await _addressService.DeleteAddressByUserIdAsync(id);
            return NoContent();
        }
    }
}