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
                startDate = source.startDate,
                endDate = source.endDate
            };
            return date;
        }
    }
}
