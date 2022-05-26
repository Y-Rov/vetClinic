using Core.Entities;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class SalaryViewModelMapper : IViewModelMapper<SalaryViewModel, Salary>
    {
        public Salary Map(SalaryViewModel source)
        {
            var salaryViewModel = new Salary
            {
                EmployeeId = source.EmployeeId,
                Value = source.Value
            };
            return salaryViewModel;
        }
    }
}
