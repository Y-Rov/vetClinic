using Core.Entities;
using Core.ViewModels.AddressViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AddressMappers
{
    public class EnumerableAddressViewModelMapper : IEnumerableViewModelMapper<IEnumerable<Address>, IEnumerable<AddressViewModel>>
    {
        private readonly IViewModelMapper<Address, AddressViewModel> _readMapper;

        public EnumerableAddressViewModelMapper(IViewModelMapper<Address, AddressViewModel> readMapper)
        {
            _readMapper = readMapper;
        }

        public IEnumerable<AddressViewModel> Map(IEnumerable<Address> source)
        {
            var readViewModels = source.Select(address => _readMapper.Map(address));
            return readViewModels;
        }
    }
}