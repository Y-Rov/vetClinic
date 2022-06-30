using Core.Models;
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
                startDate = source.startDate,
                endDate = source.endDate
            };
            return dateViewModel;
        }
    }
}
