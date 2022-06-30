using Core.Models.Finance;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class ExpencesMapper : IViewModelMapper<IEnumerable<Expences>, IEnumerable<ExpencesViewModel>>
    {
        public IEnumerable<ExpencesViewModel> Map(IEnumerable<Expences> source)
        {
            var expencesViewModels = source.Select(GetExpenceViewModel).ToList();
            return expencesViewModels;
        }
        public ExpencesViewModel GetExpenceViewModel(Expences source)
        {
            var expenceViewModel = new ExpencesViewModel()
            {
                EmployeeName = source.EmployeeName,
                SalaryValue = source.SalaryValue,
                Premium = source.Premium
            };
            return expenceViewModel;
        }
    }
}
