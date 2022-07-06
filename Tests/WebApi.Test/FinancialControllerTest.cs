using Core.Entities;
using Core.Exceptions;
using Core.Models.Finance;
using Core.ViewModels.SalaryViewModel;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class FinancialControllerTest : IClassFixture<FinancialControllerFixture>
    {
        public FinancialControllerTest(FinancialControllerFixture fixtire)
        {
            _fixture = fixtire;
        }
        private readonly FinancialControllerFixture _fixture;

        [Fact]
        public async Task GetSalaryByEmployeeId_whenIdIsCorrect_thenStatusCodeOkReturned()
        {
            //Arrange
            int id = 1;
            var salary = new Salary()
            {
                Id = id,
                EmployeeId = id,
                Value = 10,
                Date = DateTime.Now
            };

            var salaryViewModel = new SalaryViewModel()
            {
                Id = id,
                Value = 10
            };

            var employee = new User()
            {
                Id = id,
                FirstName = "Vasia",
                LastName = "Vasiliew"
            };
            var salaryWithEmployeeNameViewModel = new SalaryViewModel()
            {
                Id = id,
                Name = "Vasia Vasiliew",
                Value = 10
            };

            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetSalaryByUserIdAsync(It.Is<int>(x =>
                        x == salary.EmployeeId)))
                .ReturnsAsync(salary);

            _fixture.MockSalaryViewModel
                .Setup(mapper =>
                    mapper.Map(It.Is<Salary>(x =>
                        x == salary)))
                .Returns(salaryViewModel);

            _fixture.MockUserService
                .Setup(service =>
                    service.GetUserByIdAsync(It.Is<int>(x =>
                        x == employee.Id)))
                .ReturnsAsync(employee);

            //Act

            var result = await _fixture.MockFinancialController.GetAsync(id);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(salaryWithEmployeeNameViewModel.Id, result.Id);
            Assert.Equal(salaryWithEmployeeNameViewModel.Name, result.Name);
            Assert.Equal(salaryWithEmployeeNameViewModel.Value, result.Value);
        }

        [Fact]
        public async Task GetSalaryByEmployeeId_whenIdIsIncorrect_thenNotFoundException()
        {
            //Arrange
            int id = 1;
            var salary = new Salary()
            {
                Id = 2,
                EmployeeId = 2,
                Value = 10,
                Date = DateTime.Now
            };

            var salaryViewModel = new SalaryViewModel()
            {
                Id = 2,
                Value = 10
            };

            var employee = new User()
            {
                Id = 2,
                FirstName = "Vasia",
                LastName = "Vasiliew"
            };
            var salaryWithEmployeeNameViewModel = new SalaryViewModel()
            {
                Id = 2,
                Name = "Vasia Vasiliew",
                Value = 10
            };

            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetSalaryByUserIdAsync(It.Is<int>(x =>
                        x != salary.EmployeeId)))
                .Throws(new NotFoundException());

            _fixture.MockSalaryViewModel
                .Setup(mapper =>
                     mapper.Map(It.Is<Salary>(x =>
                        x == salary)))
                .Returns(salaryViewModel);

            _fixture.MockUserService
                .Setup(service =>
                    service.GetUserByIdAsync(It.Is<int>(x =>
                        x == employee.Id)))
                .ReturnsAsync(employee);

            //Act
            await Assert.ThrowsAsync<NotFoundException>(async () => await _fixture.MockFinancialController.GetAsync(id));

        }

        [Fact]
        public async Task GetAllSalary_whenSalaryListIsNotEmpty_thenStatusCoseOkReturned()
        {
            //Arrange
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

            var user1 = new User()
            {
                Id = 1,
                FirstName = "A",
                LastName = "B"
            };
            var user2 = new User()
            {
                Id = 2,
                FirstName = "C",
                LastName = "D"
            };
            var user3 = new User()
            {
                Id = 3,
                FirstName = "E",
                LastName = "F"
            };

            var expected = new List<SalaryViewModel>()
            {
                new SalaryViewModel()
                {
                    Id = 1,
                    Name = "A B",
                    Value = 10
                },
                new SalaryViewModel()
                {
                    Id = 2,
                    Name = "C D",
                    Value = 20
                },
                new SalaryViewModel()
                {
                    Id = 3,
                    Name = "E F",
                    Value = 30
                }
            };

            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetSalaryAsync(null))
                .ReturnsAsync(salaryList);

            _fixture.MockListSalaryViewModels
                .Setup(mapper =>
                mapper.Map(It.Is<IEnumerable<Salary>>(p => p.Equals(salaryList))))
            .Returns(salaryViewModelList);

            _fixture.MockUserService
                .Setup(service =>
                    service.GetUserByIdAsync(It.Is<int>(x => x == user1.Id)))
                .ReturnsAsync(user1);
            _fixture.MockUserService
                .Setup(service =>
                    service.GetUserByIdAsync(It.Is<int>(x => x == user2.Id)))
                .ReturnsAsync(user2);
            _fixture.MockUserService
                .Setup(service =>
                    service.GetUserByIdAsync(It.Is<int>(x => x == user3.Id)))
                .ReturnsAsync(user3);

            //Act

            var result = await _fixture.MockFinancialController.GetAsync();

            //Assert

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllSalary_whenSalaryListIsEmpty_thenStatusCoseOkReturned()
        {
            var salary = new List<Salary>();
            var salaryViewModels = new List<SalaryViewModel>();
            //Arrange
            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetSalaryAsync(null))
                .ReturnsAsync(salary);

            _fixture.MockListSalaryViewModels
                .Setup(mapper =>
                    mapper.Map(It.Is<IEnumerable<Salary>>(x => x.Equals(salary))))
                .Returns(salaryViewModels);

            //Act

            var result = await _fixture.MockFinancialController.GetAsync();

            //Assert

            Assert.NotNull(result);
            Assert.Equal(salaryViewModels, result);
        }

        [Fact]
        public async Task GetEmployees_whenEmployeesListIsNotEmpty_thenStatusOkReturned()
        {
            //Arrange
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

            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetEmployeesWithoutSalary())
                .ReturnsAsync(employees);

            _fixture.MockListEmployees
                .Setup(mapper =>
                    mapper.Map(It.Is<IEnumerable<User>>(x => x.Equals(employees))))
            .Returns(employeesViewModels);


            //Act
            var result = await _fixture.MockFinancialController.GetEmployeesAsync();

            //Assert

            Assert.NotNull(result);
            Assert.Equal(employeesViewModels, result);
        }

        [Fact]
        public async Task GetEmployees_whenEmployeesListIsEmpty_thenStatusOkReturned()
        {
            //Arrange
            var employees = new List<User>();

            var employeesViewModels = new List<EmployeeViewModel>();

            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetEmployeesWithoutSalary())
                .ReturnsAsync(employees);

            _fixture.MockListEmployees
                .Setup(mapper =>
                    mapper.Map(It.Is<IEnumerable<User>>(x => x.Equals(employees))))
            .Returns(employeesViewModels);


            //Act
            var result = await _fixture.MockFinancialController.GetEmployeesAsync();

            //Assert

            Assert.NotNull(result);
            Assert.Equal(employeesViewModels, result);
        }

        [Fact]
        public async Task CreateSalary_WhenSalaryIsNotExist_thenStatusOkReturned()
        {
            //Arrange
            var salary = new Salary()
            {
                Id = 1,
                EmployeeId = 1,
                Value = 10
            };

            var salaryViewModel = new SalaryViewModel()
            {
                Id = 1,
                Value = 10
            };

            _fixture.MockSalary
                .Setup(mapper =>
                    mapper.Map(It.IsAny<SalaryViewModel>()))
                .Returns(salary);

            _fixture.MockFinancialService
                .Setup(service =>
                    service.CreateSalaryAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            //Act
            await _fixture.MockFinancialController.PostAsync(salaryViewModel);
            //Assert

            _fixture.MockFinancialService.Verify();
        }

        [Fact]
        public async Task CreateSalary_WhenSalaryIsExist_thenBadRequestException()
        {
            //Arrange
            var salary = new Salary()
            {
                Id = 1,
                EmployeeId = 1,
                Value = 10
            };

            var salaryViewModel = new SalaryViewModel()
            {
                Id = 1,
                Value = 10
            };

            _fixture.MockSalary
                .Setup(mapper =>
                    mapper.Map(It.IsAny<SalaryViewModel>()))
                .Returns(salary);

            _fixture.MockFinancialService
                .Setup(service =>
                    service.CreateSalaryAsync(It.IsAny<Salary>()))
                .Throws(new BadRequestException());
            //Act
            //Assert
            await Assert.ThrowsAsync<BadRequestException>(async ()=> await _fixture.MockFinancialController.PostAsync(salaryViewModel));      
            
        }

        [Fact]
        public async Task DeleteSalary_WhenSalaryIsExist_thenStatusOkReturned()
        {
            //Arrange
            int id = 1;
            _fixture.MockFinancialService
                .Setup(service=>
                    service.DeleteSalaryByUserIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            //Act
            await _fixture.MockFinancialController.DeleteAsync(id);
            //Assert
            _fixture.MockFinancialService.Verify();
        }

        [Fact]
        public async Task DeleteSalary_WhenSalaryIsNotExist_thenNotFoundException()
        {
            //Arrange
            int id = 1;
            _fixture.MockFinancialService
                .Setup(service =>
                    service.DeleteSalaryByUserIdAsync(It.IsAny<int>()))
                .Throws(new NotFoundException());
            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async ()=> await _fixture.MockFinancialController.DeleteAsync(id));
        }

        [Fact]
        public async Task UpdateSalary_whenSalaryIsExist_thenStatusOkReturned()
        {
            //Arrange
            var salary = new Salary()
            {
                Id = 1,
                EmployeeId = 1,
                Value = 10
            };
            var salaryViewModel = new SalaryViewModel()
            {
                Id = 1,
                Value = 10
            };

            _fixture.MockSalary
                .Setup(mapper =>
                    mapper.Map(It.IsAny<SalaryViewModel>()))
                .Returns(salary);
            _fixture.MockFinancialService
                .Setup(service=>
                    service.UpdateSalaryAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            await _fixture.MockFinancialController.PutAsync(salaryViewModel);
            //Assert
            _fixture.MockFinancialService.Verify();
        }

        [Fact]
        public async Task UpdateSalary_whenSalaryIsNotExist_thenNotFoundException()
        {
            //Arrange
            var salary = new Salary()
            {
                Id = 1,
                EmployeeId = 1,
                Value = 10
            };
            var salaryViewModel = new SalaryViewModel()
            {
                Id = 1,
                Value = 10
            };

            _fixture.MockSalary
                .Setup(mapper =>
                    mapper.Map(It.IsAny<SalaryViewModel>()))
                .Returns(salary);
            _fixture.MockFinancialService
                .Setup(service =>
                    service.UpdateSalaryAsync(It.IsAny<Salary>()))
                .Throws(new NotFoundException());

            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async()=> await _fixture.MockFinancialController.PutAsync(salaryViewModel));
        }

        [Fact]
        public async Task UpdateSalary_whenUpdateWithTheSameValue_thenBadRequestException()
        {
            //Arrange
            var salary = new Salary()
            {
                Id = 1,
                EmployeeId = 1,
                Value = 10
            };
            var salaryViewModel = new SalaryViewModel()
            {
                Id = 1,
                Value = 10
            };

            _fixture.MockSalary
                .Setup(mapper =>
                    mapper.Map(It.IsAny<SalaryViewModel>()))
                .Returns(salary);
            _fixture.MockFinancialService
                .Setup(service =>
                    service.UpdateSalaryAsync(It.IsAny<Salary>()))
                .Throws(new BadRequestException());

            //Act
            //Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await _fixture.MockFinancialController.PutAsync(salaryViewModel));
        }

        [Fact]
        public async Task GetFinancialStatement_whenFinancialStatementListIsNotEmpty_thenReturnOk()
        {
            //Arrange
            var date = new Date()
            {
                StartDate = new DateTime(2022, 5, 1),
                EndDate = new DateTime(2022, 6, 1)
            };

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


            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetFinancialStatement(It.Is<Date>(x => x.Equals(date))))
                .ReturnsAsync(finStatement);
            _fixture.MockFinancialStatementViewModel
                .Setup(mapper=>
                    mapper.Map(It.IsAny<IEnumerable<FinancialStatement>>()))
                .Returns(finStatementVM);
            //Act
            var result = await _fixture.MockFinancialController.GetFinancialStatementAsync(date);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(finStatementVM,result);
        }

        [Fact]
        public async Task GetFinancialStatement_whenFinancialStatementListIsEmpty_thenReturnOk()
        {
            //Arrange

            var date = new Date()
            {
                StartDate = new DateTime(2022, 5, 1),
                EndDate = new DateTime(2022, 6, 1)
            };

            var finStatement = new List<FinancialStatement>();

            var finStatementVM = new List<FinancialStatementForMonthViewModel>();


            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetFinancialStatement(It.Is<Date>(x => x.Equals(date))))
                .ReturnsAsync(finStatement);
            _fixture.MockFinancialStatementViewModel
                .Setup(mapper =>
                    mapper.Map(It.IsAny<IEnumerable<FinancialStatement>>()))
                .Returns(finStatementVM);
            //Act
            var result = await _fixture.MockFinancialController.GetFinancialStatementAsync(date);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(finStatementVM, result);
        }
    }
}
