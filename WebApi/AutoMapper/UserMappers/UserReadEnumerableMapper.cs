using Core.Entities;
using Core.ViewModels.User;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.UserMappers
{
    public class UserReadEnumerableMapper : IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<UserReadViewModel>>
    {
        private readonly IViewModelMapper<User, UserReadViewModel> _readMapper;

        public UserReadEnumerableMapper(IViewModelMapper<User, UserReadViewModel> readMapper)
        {
            _readMapper = readMapper;
        }

        public IEnumerable<UserReadViewModel> Map(IEnumerable<User> source)
        {
            var readViewModels = source.Select(u => _readMapper.Map(u)).ToList();
            return readViewModels;
        }
    }
}
