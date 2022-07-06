using Core.Models.Finance;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class DateMapper : IViewModelMapper<Date, DateViewModel>
    {
        public DateViewModel Map(Date source)
        {

            var dateViewModel = new DateViewModel()
            {
                StartDate = source.StartDate,
                EndDate = source.EndDate
            };
            return dateViewModel;
        }
    }
}
