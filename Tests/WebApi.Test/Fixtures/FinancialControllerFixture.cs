using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models.Finance;
using Core.ViewModels.SalaryViewModel;
using Moq;
using WebApi.AutoMapper.Interface;
using WebApi.Controllers;

namespace WebApi.Test.Fixtures
{
    public class FinancialControllerFixture
    {
        public FinancialControllerFixture()
        {
            var fixture =
                new Fixture().Customize(new AutoMoqCustomization());

            MockFinancialService = fixture.Freeze<Mock<IFinancialService>>();
            MockUserService = fixture.Freeze<Mock<IUserService>>();
            MockSalaryViewModel = fixture.Freeze<Mock<IViewModelMapper<Salary, SalaryViewModel>>>();
            MockSalary = fixture.Freeze<Mock<IViewModelMapper<SalaryViewModel, Salary>>>();
            MockListSalaryViewModels = fixture.Freeze<Mock<IViewModelMapper<IEnumerable<Salary>, IEnumerable<SalaryViewModel>>>>();
            MockListEmployees = fixture.Freeze<Mock<IViewModelMapper<IEnumerable<User>, IEnumerable<EmployeeViewModel>>>>();
            MockFinancialStatementViewModel = fixture.Freeze<Mock<IViewModelMapper<IEnumerable<FinancialStatement>,
                IEnumerable<FinancialStatementForMonthViewModel>>>>();


            MockFinancialController = new FinancialController(
                MockFinancialService.Object,
                MockUserService.Object,
                MockSalaryViewModel.Object,
                MockSalary.Object,
                MockListSalaryViewModels.Object,
                MockListEmployees.Object,
                MockFinancialStatementViewModel.Object
                );

            UserId = 1;
            SalaryModel = GenerateSalary();
            SalaryViewModel = GenerateSalaryViewModel();
            Employee = GenerateEmployee();
            SalaryWithNameViewModel = GenarateSalaryWithName();
            SalaryList = GenerateSalaryList();
            SalaryVMList = GenerateSalaryVMList();
            SalaryEmptyList = GenerateEmptyList();
            SalaryVMEmptyList = GenerateVMEmptyList();
            EmployeeList = GenerateEmployeeList();
            EmployeeVMList = GenerateEmployeeVMList();
            EmployeeEmptyList = GenerateEmployeeEmptyList();
            EmployeeVMEmptyList = GenerateEmployeeVMEmptyList();
            Date = GenerateDate();
            FinStatList = GenerateFinancialStatement();
            FinStatVMList = GenerateFinancialStatementVM();
            FinStatEmpty = GenerateEmptyStatements();
            FinStatVMEmpty = GenerateEmptyFinStatVM();
        }

        public FinancialController MockFinancialController { get; }
        public Mock<IFinancialService> MockFinancialService { get; }
        public Mock<IUserService> MockUserService { get; }
        public Mock<IViewModelMapper<Salary, SalaryViewModel>> MockSalaryViewModel { get; }
        public Mock<IViewModelMapper<SalaryViewModel, Salary>> MockSalary { get; }
        public Mock<IViewModelMapper<IEnumerable<Salary>, IEnumerable<SalaryViewModel>>> MockListSalaryViewModels { get; }
        public Mock<IViewModelMapper<IEnumerable<User>, IEnumerable<EmployeeViewModel>>> MockListEmployees { get; }
        public Mock<IViewModelMapper<IEnumerable<FinancialStatement>,
            IEnumerable<FinancialStatementForMonthViewModel>>> MockFinancialStatementViewModel { get; }

        public int UserId {get;}
        public Salary SalaryModel { get;}
        public SalaryViewModel SalaryViewModel { get;}
        public User Employee { get;}
        public SalaryViewModel SalaryWithNameViewModel { get; }
        public IEnumerable<Salary> SalaryList { get; }
        public IEnumerable<SalaryViewModel> SalaryVMList { get; }
        public IEnumerable<Salary> SalaryEmptyList { get; }
        public IEnumerable<SalaryViewModel> SalaryVMEmptyList { get; }
        public IEnumerable<User> EmployeeList { get; }
        public IEnumerable<EmployeeViewModel> EmployeeVMList { get; }
        public IEnumerable<User> EmployeeEmptyList { get; }
        public IEnumerable<EmployeeViewModel> EmployeeVMEmptyList { get; }
        public DatePeriod Date { get; }
        public IEnumerable<FinancialStatement> FinStatList { get; }
        public IEnumerable<FinancialStatementForMonthViewModel> FinStatVMList { get; }
        public IEnumerable<FinancialStatement> FinStatEmpty { get; }
        public IEnumerable<FinancialStatementForMonthViewModel> FinStatVMEmpty { get; }

        private Salary GenerateSalary()
        {
            var salary = new Salary()
            {
                Id = UserId,
                EmployeeId = UserId,
                Value = 10,
                Date = DateTime.Now
            };
            return salary;
        }
        private SalaryViewModel GenerateSalaryViewModel()
        {
            var salaryViewModel = new SalaryViewModel()
            {
                Id = UserId,
                Value = 10
            };
            return salaryViewModel;
        }
        private User GenerateEmployee()
        {
            var employee = new User()
            {
                Id = UserId,
                FirstName = "Vasia",
                LastName = "Vasiliew"
            };
            return employee;
        }
        private SalaryViewModel GenarateSalaryWithName()
        {
            var salaryWithEmployeeNameViewModel = new SalaryViewModel()
            {
                Id = UserId,
                Name = "Vasia Vasiliew",
                Value = 10
            };
            return salaryWithEmployeeNameViewModel;
        }
        private IEnumerable<Salary> GenerateSalaryList()
        {
            var salaryList = new List<Salary>()
            {
                new Salary()
                {
                    Id = 1,
                    EmployeeId = 1,
                    Value = 10
                },
                new Salary()
                {
                    Id = 2,
                    EmployeeId = 2,
                    Value = 20
                },
                new Salary()
                {
                    Id = 3,
                    EmployeeId = 3,
                    Value = 30
                }
            };
            return salaryList;
        }
        private IEnumerable<SalaryViewModel> GenerateSalaryVMList()
        {
            var salaryViewModelList = new List<SalaryViewModel>()
            {
                new SalaryViewModel()
                {
                    Id = 1,
                    Value = 10
                },
                new SalaryViewModel()
                {
                    Id = 2,
                    Value = 20
                },
                new SalaryViewModel()
                {
                    Id = 3,
                    Value = 30
                }
            };
            return salaryViewModelList;
        }
        private IEnumerable<Salary> GenerateEmptyList()
        {
            return new List<Salary>();
        }
        private IEnumerable<SalaryViewModel> GenerateVMEmptyList()
        {
            return new List<SalaryViewModel>();
        }
        private IEnumerable<User> GenerateEmployeeList()
        {
            var employees = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    FirstName = "A",
                    LastName = "B"
                },
                new User()
                {
                    Id = 2,
                    FirstName = "A",
                    LastName = "B"
                },
                new User()
                {
                    Id = 3,
                    FirstName = "A",
                    LastName = "B"
                }
            };
            return employees;
        }
        private IEnumerable<EmployeeViewModel> GenerateEmployeeVMList()
        {
            var employeesViewModels = new List<EmployeeViewModel>
            {
                new EmployeeViewModel()
                {
                    Id = 1,
                    FirstName = "A",
                    LastName = "B"
                },
                new EmployeeViewModel()
                {
                    Id = 2,
                    FirstName = "A",
                    LastName = "B"
                },
                new EmployeeViewModel()
                {
                    Id = 3,
                    FirstName = "A",
                    LastName = "B"
                }
            };
            return employeesViewModels;
        }
        private IEnumerable<User> GenerateEmployeeEmptyList()
        {
            return new List<User>();
        }
        private IEnumerable<EmployeeViewModel> GenerateEmployeeVMEmptyList()
        {
            return new List<EmployeeViewModel>();
        }
        private DatePeriod GenerateDate()
        {
            var date = new DatePeriod()
            {
                StartDate = new DateTime(2022, 5, 1),
                EndDate = new DateTime(2022, 6, 1)
            };
            return date;
        }
        private IEnumerable<FinancialStatement> GenerateFinancialStatement()
        {
            var finStatement = new List<FinancialStatement>()
            {
                new FinancialStatement()
                {
                    Month = "first",
                    TotalIncomes =1,
                    TotalExpences=1
                },
                new FinancialStatement()
                {
                    Month = "second",
                    TotalIncomes =2,
                    TotalExpences=2
                }
            };
            return finStatement;
        }
        private IEnumerable<FinancialStatementForMonthViewModel> GenerateFinancialStatementVM()
        {
            var finStatementVM = new List<FinancialStatementForMonthViewModel>()
            {
                new FinancialStatementForMonthViewModel()
                {
                    Month = "first",
                    TotalIncomes =1,
                    TotalExpences=1
                },
                new FinancialStatementForMonthViewModel()
                {
                    Month = "second",
                    TotalIncomes =2,
                    TotalExpences=2
                }
            };
            return finStatementVM;
        }
        private IEnumerable<FinancialStatement> GenerateEmptyStatements()
        {
            return new List<FinancialStatement>();
        }
        private IEnumerable<FinancialStatementForMonthViewModel> GenerateEmptyFinStatVM()
        {
            return new List<FinancialStatementForMonthViewModel>();
        }
    }
}
