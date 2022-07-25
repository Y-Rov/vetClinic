using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models.Finance;
using Core.Paginator;
using Core.Paginator.Parameters;
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
            EmployeeList = GenerateEmployeeList();
            AppointmentList = GenerateAppoinmentList();
            SalaryList = GenerateSalaryList();
            AppoinmentEmptyList = GenerateAppoinmentEmptyList();
            SalaryWithZeroValue = GenerateSalaryWithZeroValue();
            SalaryWithValue = 2;
            UserOne = GenerateFirstUser();
            UserTwo = GenerateSecondUser();
            ProcedureOne = GenerateFirstProcedure();
            ProcedureTwo = GenerateSecondProcedure();
            SalaryParametrs = GenerateSalaryParametrs();
            FinancialStatementParameters = GenerateFinancialStatementParametrs();
            SalaryOne = generateSalaryOne();
            SalaryTwo = generateSalaryTwo();
        }

        public IFinancialService MockFinancialService { get; }
        public Mock<ISalaryRepository> MockSalaryRepository { get; }
        public Mock<IUserRepository> MockUserRepository { get; }
        public Mock<IAppointmentRepository> MockAppointmentRepository { get; }
        public Mock<IProcedureRepository> MockProcedureRepository { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }

        public int UserId { get; }
        public Salary SalaryModel { get; }
        public PagedList<Salary> SalaryListFromRepo { get; }
        public PagedList<Salary> SalaryEmptyList { get; }
        public Salary UpdatedSalary { get; }
        public List<User> EmployeeList { get; }
        public List<Appointment> AppointmentList { get; }
        public PagedList<Salary> SalaryList { get; }
        public List<Appointment> AppoinmentEmptyList { get; }
        public Salary SalaryWithZeroValue { get; }
        public int SalaryWithValue { get; }
        public User UserOne { get; }
        public User UserTwo { get; }
        public Procedure ProcedureOne { get; }
        public Procedure ProcedureTwo { get; }
        public SalaryParametrs SalaryParametrs { get; }
        public FinancialStatementParameters FinancialStatementParameters { get; }
        public Salary SalaryOne { get; }
        public Salary SalaryTwo { get; }

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
        private PagedList<Salary> GenerateSalaryListFromRepo()
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
            return new PagedList<Salary>(salaryList,salaryList.Count, 1, salaryList.Count);
        }
        private PagedList<Salary> GenerateSalaryEmptyList()
        {
            var res = new PagedList<Salary>(new List<Salary>(), 0, 1, 1);
            return res;
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
        private PagedList<Salary> GenerateSalaryList()
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
            return new PagedList<Salary>(salaries,salaries.Count,1,salaries.Count);
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
        private User GenerateFirstUser() {
            var userOne = new User
            {
                Id = 1,
                FirstName = "frst",
                LastName = "scnd"
            };
            return userOne;
        }
        private User GenerateSecondUser() {
            var userTwo = new User
            {
                Id = 2,
                FirstName = "dfg",
                LastName = "erg"
            };
            return userTwo;
        }
        private Procedure GenerateFirstProcedure() 
        {
            var procedureOne = new Procedure()
            {
                Id = 1,
                Name = "One",
                Cost = 5
            };
            return procedureOne;
        }
        private Procedure GenerateSecondProcedure() {

            var procedureTwo = new Procedure()
            {
                Id = 2,
                Name = "Two",
                Cost = 10
            };
            return procedureTwo;
        }
        private SalaryParametrs GenerateSalaryParametrs()
        {
            var res = new SalaryParametrs()
            {
                PageNumber = 1,
                PageSize = 5
            };
            return res;
        }
        private FinancialStatementParameters GenerateFinancialStatementParametrs()
        {
            var date = new DatePeriod()
            {
                StartDate = new DateTime(2022, 5, 1),
                EndDate = new DateTime(2022, 6, 1)
            };
            var res = new FinancialStatementParameters()
            {
                PageNumber = 1,
                PageSize = 5,
                Date = date
            };
            return res;
        }
        private Salary generateSalaryOne()
        {
            var salary = new Salary
            {
                EmployeeId = 1,
                Value = 10,
                Date = new DateTime(2022, 4, 1)
            };
            return salary;
        }
        private Salary generateSalaryTwo()
        {
            var salary = new Salary
            {
                EmployeeId = 2,
                Value = 10,
                Date = new DateTime(2022, 4, 20)
            };
            return salary;
        }
    }
}
