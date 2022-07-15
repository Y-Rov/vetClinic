using Core.Models;
using Core.ViewModels.ExceptionViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ExceptionMappers
{
    public class ExceptionStatsMapper : IEnumerableViewModelMapper<IEnumerable<ExceptionStats>, IEnumerable<ExceptionStatsReadViewModel>>
    {
        public IEnumerable<ExceptionStatsReadViewModel> Map(IEnumerable<ExceptionStats> source)
    {
        var exceptionStatsReadViewModels = source.Select(GetExceptionEntityReadViewModel).ToList();
        return exceptionStatsReadViewModels;
    }

    private ExceptionStatsReadViewModel GetExceptionEntityReadViewModel(ExceptionStats exception)
    {
        var appointmentViewModel = new ExceptionStatsReadViewModel()
        {
           Name = exception.Name,
           Count = exception.Count,
        };

        return appointmentViewModel;
    }
    }
}
