using Core.Models.Finance;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class DateViewModelMapper : IViewModelMapper<DateViewModel, Date>
    {
        public Date Map(DateViewModel source)
        {

            var date = new Date()
            {
                StartDate = source.StartDate.ToLocalTime(),
                EndDate = source.EndDate.ToLocalTime()
            };
            return date;
        }
    }
}
