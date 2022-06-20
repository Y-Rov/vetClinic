using Core.Entities;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class EmployeesMapper : IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<EmployeeViewModel>>
    {
        public IEnumerable<EmployeeViewModel> Map(IEnumerable<User> source)
        {
            var employeeViewModels = source.Select(GetEmployeeViewModel).ToList();
            return employeeViewModels;
        }

        private EmployeeViewModel GetEmployeeViewModel(User employee)
        {
            var employeeViewModel = new EmployeeViewModel()
            {
                Id = employee.Id,
                Name = employee.FirstName + " " + employee.LastName
            };

            return employeeViewModel;
        }
    }

}
