using Core.Entities;
using Core.Paginator;
using Core.ViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AppointmentMappers
{
    public class AppointmentPagedMapper : IEnumerableViewModelMapper<PagedList<Appointment>, PagedReadViewModel<AppointmentReadViewModel>>
    {
        readonly IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentReadViewModel>>
            _listMapper;

        public AppointmentPagedMapper(IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentReadViewModel>> listMapper)
        {
            _listMapper = listMapper;
        }

        public PagedReadViewModel<AppointmentReadViewModel> Map(PagedList<Appointment> source)
        {
            return new PagedReadViewModel<AppointmentReadViewModel>
            {
                CurrentPage = source.CurrentPage,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                HasPrevious = source.HasPrevious,
                HasNext = source.HasNext,
                TotalPages = source.TotalPages,
                Entities = _listMapper.Map(source.AsEnumerable())
            };
        }
    }


}
