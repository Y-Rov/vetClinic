using Core.Entities;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class SalaryMapper : IViewModelMapper<Salary, SalaryViewModel>
    {
        public SalaryViewModel Map(Salary source)
        {
            var salary = new SalaryViewModel
            {
                EmployeeId = source.EmployeeId,
                Value = source.Value
            };
            return salary;
        }
    }
}
