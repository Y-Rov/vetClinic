using Core.Entities;
using Core.ViewModels.AddressViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AddressMappers
{
    public class AddressReadEnumerableViewModelMapper 
        : IEnumerableViewModelMapper<IEnumerable<Address>, IEnumerable<AddressCreateViewModel>>
    {
        private readonly IViewModelMapper<Address, AddressCreateViewModel> _readMapper;

        public AddressReadEnumerableViewModelMapper(IViewModelMapper<Address, AddressCreateViewModel> readMapper)
        {
            _readMapper = readMapper;
        }

        public IEnumerable<AddressCreateViewModel> Map(IEnumerable<Address> source)
        {
            var readViewModels = source.Select(address => _readMapper.Map(address));
            return readViewModels;
        }
    }
}