using Core.Entities;
using Core.ViewModels.AddressViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AddressMappers
{
    public class AddressReadEnumerableViewModelMapper 
        : IEnumerableViewModelMapper<IEnumerable<Address>, IEnumerable<AddressCreateReadViewModel>>
    {
        private readonly IViewModelMapper<Address, AddressCreateReadViewModel> _readMapper;

        public AddressReadEnumerableViewModelMapper(IViewModelMapper<Address, AddressCreateReadViewModel> readMapper)
        {
            _readMapper = readMapper;
        }

        public IEnumerable<AddressCreateReadViewModel> Map(IEnumerable<Address> source)
        {
            var readViewModels = source.Select(address => _readMapper.Map(address));
            return readViewModels;
        }
    }
}