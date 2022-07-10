using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models.Finance;
using Moq;

namespace Application.Test.Fixtures
{
    public class FinancialServiceFixture
    {
        public FinancialServiceFixture()
        {
            var fixture =
                new Fixture().Customize(new AutoMoqCustomization());

            MockSalaryRepository = fixture.Freeze<Mock<ISalaryRepository>>();
            MockUserRepository = fixture.Freeze<Mock<IUserRepository>>();
            MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();
            MockAppointmentRepository = fixture.Freeze<Mock<IAppointmentRepository>>();
            MockProcedureRepository = fixture.Freeze<Mock<IProcedureRepository>>();

            MockFinancialService = new FinancialService(
                MockSalaryRepository.Object,
                MockUserRepository.Object,
                MockLoggerManager.Object,
                MockAppointmentRepository.Object,
                MockProcedureRepository.Object);

            UserId = 1;
            SalaryModel = GenerateSalary();
            SalaryListFromRepo = GenerateSalaryListFromRepo();
            SalaryEmptyList = GenerateSalaryEmptyList();
            UpdatedSalary = GenerateUpdatedSalary();
            EmployeeIdList = GenerateEmployeeIdList();
            EmployeeList = GenerateEmployeeList();
            Date = GenerateDate();
            AppointmentList = GenerateAppoinmentList();
            SalaryList = GenerateSalaryList();
            AppoinmentEmptyList = GenerateAppoinmentEmptyList();
            SalaryWithZeroValue = GenerateSalaryWithZeroValue();
            SalaryWithValue = 2;
        }

        public IFinancialService MockFinancialService { get; }
        public Mock<ISalaryRepository> MockSalaryRepository { get; }
        public Mock<IUserRepository> MockUserRepository { get; }
        public Mock<IAppointmentRepository> MockAppointmentRepository { get; }
        public Mock<IProcedureRepository> MockProcedureRepository { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }

        public int UserId { get; }
        public Salary SalaryModel { get; }
        public List<Salary> SalaryListFromRepo { get; }
        public List<Salary> SalaryEmptyList { get; }
        public Salary UpdatedSalary { get; }
        public List<int> EmployeeIdList { get; }
        public List<User> EmployeeList { get; }
        public DatePeriod Date { get; }
        public List<Appointment> AppointmentList { get; }
        public List<Salary> SalaryList { get; }
        public List<Appointment> AppoinmentEmptyList { get; }
        public Salary SalaryWithZeroValue { get; }
        public int SalaryWithValue { get; }

        private Salary GenerateSalary()
        {
            var salary = new Salary
            {
                Id = 1,
                EmployeeId = 1,
                Value = 10
            };
            return salary;
        }
        private List<Salary> GenerateSalaryListFromRepo()
        {
            var salaryList = new List<Salary>()
            {
                new Salary()
                {
                    EmployeeId = 1,
                    Value = 10
                },
                new Salary()
                {
                    EmployeeId = 2,
                    Value = 0
                },
                new Salary()
                {
                    EmployeeId = 3,
                    Value = 30
                }
            };
            return salaryList;
        }
        private List<Salary> GenerateSalaryEmptyList()
        {
            return new List<Salary>();
        }
        private Salary GenerateUpdatedSalary()
        {
            var salary = new Salary
            {
                Id = 1,
                EmployeeId = 1,
                Value = 20
            };
            return salary;
        }
        private List<int> GenerateEmployeeIdList()
        {
            var employeesId = new List<int>()
            {
                1,2,3
            };
            return employeesId;
        }
        private List<User> GenerateEmployeeList()
        {
            var listEmployees = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    FirstName = "first"
                },
                new User()
                {
                    Id = 2,
                    FirstName = "second"
                },
                new User()
                {
                    Id = 3,
                    FirstName = "third"
                }
            };
            return listEmployees;
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
        private List<Appointment> GenerateAppoinmentList()
        {
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    Id = 1,
                    AnimalId = 1,
                    Disease ="first",
                    AppointmentProcedures =new List<AppointmentProcedure>(){new AppointmentProcedure(){AppointmentId=1,ProcedureId =1} },
                    AppointmentUsers =new List<AppointmentUser>(){new AppointmentUser(){AppointmentId=1,UserId =1} }
                },
                new Appointment
                {
                    Id = 2,
                    AnimalId = 1,
                    Disease ="second",
                    AppointmentProcedures =new List<AppointmentProcedure>(){new AppointmentProcedure(){AppointmentId=2,ProcedureId =2} },
                    AppointmentUsers =new List<AppointmentUser>(){new AppointmentUser(){AppointmentId=2,UserId =2} }
                }
            };
            return appointments;
        }
        private List<Salary> GenerateSalaryList()
        {
            var salaries = new List<Salary>
            {
                new Salary()
                {
                    EmployeeId =1,
                    Value = 10,
                },
                new Salary()
                {
                    EmployeeId = 2,
                    Value =30
                }
            };
            return salaries;
        }
        private List<Appointment> GenerateAppoinmentEmptyList()
        {
            return new List<Appointment>();
        }
        private Salary GenerateSalaryWithZeroValue()
        {
            var salary = new Salary()
            {
                Id = 1,
                EmployeeId = 1,
                Value = 0
            };
            return salary;
        }
    }
}
