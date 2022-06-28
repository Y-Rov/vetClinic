using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.AddressViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IViewModelMapper<AddressCreateViewModel, Address> _addressCreateMapper;
        private readonly IViewModelMapper<Address, AddressBaseViewModel> _addressReadViewModelMapper;
        private readonly IViewModelMapperUpdater<AddressBaseViewModel, Address> _addressUpdateMapper;
        private readonly IEnumerableViewModelMapper<IEnumerable<Address>, IEnumerable<AddressCreateViewModel>>
            _addressReadEnumerableViewModelMapper;

        public AddressController(
            IAddressService addressService,
            IViewModelMapper<Address, AddressBaseViewModel> addressReadViewModelMapper,
            IViewModelMapper<AddressCreateViewModel, Address> addressCreateMapper,
            IViewModelMapperUpdater<AddressBaseViewModel, Address> addressUpdateMapper,
            IEnumerableViewModelMapper<IEnumerable<Address>, IEnumerable<AddressCreateViewModel>> addressReadEnumerableViewModelMapper)
        {
            _addressService = addressService;
            _addressCreateMapper = addressCreateMapper;
            _addressReadViewModelMapper = addressReadViewModelMapper;
            _addressUpdateMapper = addressUpdateMapper;
            _addressReadEnumerableViewModelMapper = addressReadEnumerableViewModelMapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<AddressCreateViewModel>>> GetAsync()
        {
            var addresses = await _addressService.GetAllAddressesAsync();

            var viewModels = _addressReadEnumerableViewModelMapper.Map(addresses);
            return Ok(viewModels);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<AddressBaseViewModel>> GetAsync([FromRoute] int id)
        {
            var address = await _addressService.GetAddressByUserIdAsync(id);

            var viewModel = _addressReadViewModelMapper.Map(address);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] AddressCreateViewModel addressCreateViewModel)
        {
            var address = _addressCreateMapper.Map(addressCreateViewModel);

            await _addressService.CreateAddressAsync(address);
            return NoContent();
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] AddressBaseViewModel addressBaseViewModel)
        {
            var address = await _addressService.GetAddressByUserIdAsync(id);
            _addressUpdateMapper.Map(addressBaseViewModel, address);
            
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