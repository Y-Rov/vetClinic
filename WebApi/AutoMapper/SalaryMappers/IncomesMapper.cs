using Core.Entities;
using Core.Models.Finance;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class IncomesMapper : IEnumerableViewModelMapper<IEnumerable<Income>, IEnumerable<IncomeViewModel>>
    {
        public IEnumerable<IncomeViewModel> Map(IEnumerable<Income> source)
        {
            var incomeViewModels = source.Select(GetIncomeViewModel).ToList();
            return incomeViewModels;
        }

        public IncomeViewModel GetIncomeViewModel(Income source)
        {
            var incomeViewModel = new IncomeViewModel()
            {
                AppointmenId = source.AppointmenId,
                Cost = source.Cost
            };
            return incomeViewModel;
        }
    }
}
