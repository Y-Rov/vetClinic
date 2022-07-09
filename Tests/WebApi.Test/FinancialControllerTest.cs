using Core.Entities;
using Core.Exceptions;
using Core.Models.Finance;
using Core.ViewModels.SalaryViewModel;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class FinancialControllerTest : IClassFixture<FinancialControllerFixture>, IDisposable
    {
        private readonly FinancialControllerFixture _fixture;
        private bool _disposed;
        public FinancialControllerTest(FinancialControllerFixture fixtire)
        {
            _fixture = fixtire;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _fixture.MockFinancialService.ResetCalls();
            }

            _disposed = true;
        }
        [Fact]
        public async Task GetSalaryByEmployeeId_whenIdIsCorrect_thenStatusCodeOkReturned()
        {
            //Arrange
            
            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetSalaryByUserIdAsync(It.Is<int>(x =>
                        x == _fixture.SalaryModel.EmployeeId)))
                .ReturnsAsync(_fixture.SalaryModel);

            _fixture.MockSalaryViewModel
                .Setup(mapper =>
                    mapper.Map(It.Is<Salary>(x =>
                        x == _fixture.SalaryModel)))
                .Returns(_fixture.SalaryViewModel);

            _fixture.MockUserService
                .Setup(service =>
                    service.GetUserByIdAsync(It.Is<int>(x =>
                        x == _fixture.Employee.Id)))
                .ReturnsAsync(_fixture.Employee);

            //Act

            var result = await _fixture.MockFinancialController.GetAsync(_fixture.UserId);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(_fixture.SalaryWithNameViewModel, result);
        }

        [Fact]
        public async Task GetAllSalary_whenSalaryListIsNotEmpty_thenStatusCoseOkReturned()
        {
            //Arrange

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


            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetSalaryAsync(null))
                .ReturnsAsync(_fixture.SalaryList);

            _fixture.MockListSalaryViewModels
                .Setup(mapper =>
                mapper.Map(It.Is<IEnumerable<Salary>>(p => p.Equals(_fixture.SalaryList))))
            .Returns(_fixture.SalaryVMList);

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
            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<IEnumerable<SalaryViewModel>>(result);
        }

        [Fact]
        public async Task GetAllSalary_whenSalaryListIsEmpty_thenStatusCoseOkReturned()
        {
            //Arrange
            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetSalaryAsync(null))
                .ReturnsAsync(_fixture.SalaryEmptyList);

            _fixture.MockListSalaryViewModels
                .Setup(mapper =>
                    mapper.Map(It.Is<IEnumerable<Salary>>(x => x.Equals(_fixture.SalaryEmptyList))))
                .Returns(_fixture.SalaryVMEmptyList);

            //Act

            var result = await _fixture.MockFinancialController.GetAsync();

            //Assert

            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsAssignableFrom<IEnumerable<SalaryViewModel>>(result);
        }

        [Fact]
        public async Task GetEmployees_whenEmployeesListIsNotEmpty_thenStatusOkReturned()
        {
            //Arrange

            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetEmployeesWithoutSalary())
                .ReturnsAsync(_fixture.EmployeeList);

            _fixture.MockListEmployees
                .Setup(mapper =>
                    mapper.Map(It.Is<IEnumerable<User>>(x => x.Equals(_fixture.EmployeeList))))
            .Returns(_fixture.EmployeeVMList);


            //Act
            var result = await _fixture.MockFinancialController.GetEmployeesAsync();

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<IEnumerable<EmployeeViewModel>>(result);
        }

        [Fact]
        public async Task GetEmployees_whenEmployeesListIsEmpty_thenStatusOkReturned()
        {
            //Arrange

            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetEmployeesWithoutSalary())
                .ReturnsAsync(_fixture.EmployeeEmptyList);

            _fixture.MockListEmployees
                .Setup(mapper =>
                    mapper.Map(It.Is<IEnumerable<User>>(x => x.Equals(_fixture.EmployeeEmptyList))))
            .Returns(_fixture.EmployeeVMEmptyList);


            //Act
            var result = await _fixture.MockFinancialController.GetEmployeesAsync();

            //Assert

            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsAssignableFrom<IEnumerable<EmployeeViewModel>>(result);
        }

        [Fact]
        public async Task CreateSalary_WhenSalaryIsNotExist_thenStatusOkReturned()
        {
            //Arrange
            _fixture.MockSalary
                .Setup(mapper =>
                    mapper.Map(It.Is<SalaryViewModel>(x=> x== _fixture.SalaryViewModel)))
                .Returns(_fixture.SalaryModel);

            _fixture.MockFinancialService
                .Setup(service =>
                    service.CreateSalaryAsync(It.Is<Salary>(x=> x==_fixture.SalaryModel)))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            //Act
            await _fixture.MockFinancialController.PostAsync(_fixture.SalaryViewModel);
            //Assert

            _fixture.MockFinancialService.Verify();
        }

        [Fact]
        public async Task DeleteSalary_WhenSalaryIsExist_thenStatusOkReturned()
        {
            //Arrange
            _fixture.MockFinancialService
                .Setup(service=>
                    service.DeleteSalaryByUserIdAsync(It.Is<int>(x=> x==_fixture.UserId)))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            //Act
            await _fixture.MockFinancialController.DeleteAsync(_fixture.UserId);
            //Assert
            _fixture.MockFinancialService.Verify();
        }


        [Fact]
        public async Task UpdateSalary_whenSalaryIsExist_thenStatusOkReturned()
        {
            //Arrange

            _fixture.MockSalary
                .Setup(mapper =>
                    mapper.Map(It.Is<SalaryViewModel>(x=> x==_fixture.SalaryViewModel)))
                .Returns(_fixture.SalaryModel);
            _fixture.MockFinancialService
                .Setup(service=>
                    service.UpdateSalaryAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            await _fixture.MockFinancialController.PutAsync(_fixture.SalaryViewModel);
            //Assert
            _fixture.MockFinancialService.Verify();
        }

        [Fact]
        public async Task GetFinancialStatement_whenFinancialStatementListIsNotEmpty_thenReturnOk()
        {
            //Arrange

            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetFinancialStatement(It.Is<DatePeriod>(x => x.Equals(_fixture.Date))))
                .ReturnsAsync(_fixture.FinStatList);
            _fixture.MockFinancialStatementViewModel
                .Setup(mapper=>
                    mapper.Map(It.Is<IEnumerable<FinancialStatement>>(x=> x==_fixture.FinStatList)))
                .Returns(_fixture.FinStatVMList);
            //Act
            var result = await _fixture.MockFinancialController.GetFinancialStatementAsync(_fixture.Date);
            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<IEnumerable<FinancialStatementForMonthViewModel>>(result);
        }

        [Fact]
        public async Task GetFinancialStatement_whenFinancialStatementListIsEmpty_thenReturnOk()
        {
            //Arrange

            _fixture.MockFinancialService
                .Setup(service =>
                    service.GetFinancialStatement(It.Is<DatePeriod>(x => x.Equals(_fixture.Date))))
                .ReturnsAsync(_fixture.FinStatEmpty);
            _fixture.MockFinancialStatementViewModel
                .Setup(mapper =>
                    mapper.Map(It.Is<IEnumerable<FinancialStatement>>(x=> x==_fixture.FinStatEmpty)))
                .Returns(_fixture.FinStatVMEmpty);
            //Act
            var result = await _fixture.MockFinancialController.GetFinancialStatementAsync(_fixture.Date);
            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsAssignableFrom<IEnumerable<FinancialStatementForMonthViewModel>>(result);
        }
    }
}
