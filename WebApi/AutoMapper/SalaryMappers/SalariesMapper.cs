using Core.Entities;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class SalariesMapper : IEnumerableViewModelMapper<IEnumerable<Salary>, IEnumerable<SalaryViewModel>>
    {
        public IEnumerable<SalaryViewModel> Map(IEnumerable<Salary> source)
        {
            var salaryViewModels = source.Select(GetSalaryViewModel).ToList();
            return salaryViewModels;
        }

        private SalaryViewModel GetSalaryViewModel(Salary salary)
        {
            var salaryViewModel = new SalaryViewModel()
            {
                Id = salary.EmployeeId,
                Value = salary.Value
            };

            return salaryViewModel;
        }
    }
}
