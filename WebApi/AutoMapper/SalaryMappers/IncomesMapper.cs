using Core.Entities;
using Core.Models.Finance;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class IncomesMapper : IEnumerableViewModelMapper<IEnumerable<Income>, IEnumerable<IncomeViewModel>>
    {
        readonly IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>> _internalmapper;

        public IncomesMapper(IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>> internalmapper)
        {
            _internalmapper = internalmapper;
        }

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
            incomeViewModel.ListOfProcedures = _internalmapper.Map(source.ListOfProcedures);
            return incomeViewModel;
        }
    }
}
