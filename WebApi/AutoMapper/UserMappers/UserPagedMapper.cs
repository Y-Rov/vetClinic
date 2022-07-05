using Core.Entities;
using Core.Paginator;
using Core.ViewModels;
using Core.ViewModels.User;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.UserMappers
{
    public class UserPagedMapper : IViewModelMapper<PagedList<User>, PagedReadViewModel<UserReadViewModel>>
    {
        private readonly IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<UserReadViewModel>> _userReadViewModel;
        
        public UserPagedMapper(IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<UserReadViewModel>> userReadViewModel)
        {
            _userReadViewModel = userReadViewModel;
        }

        public PagedReadViewModel<UserReadViewModel> Map(PagedList<User> source)
        {
            return new PagedReadViewModel<UserReadViewModel>()
            {
                CurrentPage = source.CurrentPage,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                HasPrevious = source.HasPrevious,
                HasNext = source.HasNext,
                TotalPages = source.TotalPages,
                Entities = _userReadViewModel.Map(source.AsEnumerable())
            };
        }
    }
}
