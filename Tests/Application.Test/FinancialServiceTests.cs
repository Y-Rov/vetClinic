using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Core.Paginator;
using Core.Paginator.Parameters;
using Microsoft.EntityFrameworkCore.Query;
using Core.Models.Finance;
using Moq;
using System.Linq.Expressions;
using Neleus.LambdaCompare;

namespace Application.Test
{
    public class FinancialServiceTests: IClassFixture<FinancialServiceFixture>, IDisposable
    {
        public FinancialServiceTests(FinancialServiceFixture fixture)
        {
            _fixture = fixture;
        }

        readonly FinancialServiceFixture _fixture;
        private bool _disposed;
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
                _fixture.MockSalaryRepository.ResetCalls();
                _fixture.MockAppointmentRepository.ResetCalls();
                _fixture.MockUserRepository.ResetCalls();
                _fixture.MockProcedureRepository.ResetCalls();
            }

            _disposed = true;
        }

        [Fact]
        public async Task CreateSalary_whenSalaryIsNotExist_thanCreate()
        {
            //Arrange

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(
                    It.IsAny<int>(), 
                    It.IsAny<string>()))
                .ReturnsAsync(()=> null)
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo=>repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();


            //Act
            await _fixture.MockFinancialService.CreateSalaryAsync(_fixture.SalaryModel);

            //Assert

            _fixture.MockLoggerManager.Verify();
            _fixture.MockSalaryRepository.Verify(repo => repo.GetById(_fixture.UserId, ""), Times.Once);
            _fixture.MockSalaryRepository.Verify(repo => repo.InsertAsync(_fixture.SalaryModel), Times.Once);
            _fixture.MockSalaryRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateSalary_whenSalaryIsExist_thanBadRequestException()
        {
            //Arrange

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(It.Is<int>(x=> x==_fixture.UserId), It.IsAny<string>()))
                .ReturnsAsync(_fixture.SalaryModel);

            //Act
            //Assert

            await Assert.ThrowsAsync<BadRequestException>(async()=> 
                await _fixture.MockFinancialService.CreateSalaryAsync(_fixture.SalaryModel));

        }

        [Fact]
        public async Task CreateSalary_whenSalaryValueIsZero_thanCreate()
        {
            //Arrange

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(It.Is<int>(x => x == _fixture.UserId), It.IsAny<string>()))
                .ReturnsAsync(_fixture.SalaryWithZeroValue);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(It.Is<int>(x => x == _fixture.UserId), It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //Act

            await _fixture.MockFinancialService.CreateSalaryAsync(_fixture.SalaryModel);

            //Assert

            _fixture.MockLoggerManager.Verify();
            _fixture.MockSalaryRepository.Verify(repo => repo.GetById(_fixture.UserId, ""), Times.Once);
            _fixture.MockSalaryRepository.Verify(repo => repo.InsertAsync(_fixture.SalaryModel), Times.Once);
            _fixture.MockSalaryRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

        }

        [Fact]
        public async Task DeleteSalary_whenSalaryIsExist_thanDelete()
        {
            //Arrange

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(
                    It.Is<int>(x=> x==_fixture.SalaryModel.EmployeeId), 
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.SalaryModel);

            _fixture.MockSalaryRepository
                .Setup(repo=> repo.InsertAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //Act

            await _fixture.MockFinancialService.DeleteSalaryByUserIdAsync(_fixture.SalaryModel.EmployeeId);

            //Assert

            _fixture.MockLoggerManager.Verify();
            _fixture.MockSalaryRepository.Verify(repo => repo.GetById(_fixture.UserId, ""), Times.Once);
            _fixture.MockSalaryRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteSalary_whenSalaryValueIsZero_thanNotFoundException()
        {
            //Arrange

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogWarn(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(
                    It.Is<int>(x=> x==_fixture.SalaryWithZeroValue.EmployeeId), 
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.SalaryWithZeroValue);

            //Act
            //Assert

            await Assert.ThrowsAsync<NotFoundException>(async() => 
                await _fixture.MockFinancialService.DeleteSalaryByUserIdAsync(_fixture.SalaryWithZeroValue.EmployeeId));
        }

        [Fact]
        public async Task DeleteSalary_whenSalaryIsNotExist_thanNotFoundException()
        {
            //Arrange

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogWarn(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(It.Is<int>(x=> x==_fixture.UserId), It.IsAny<string>()))
                .ReturnsAsync(()=>null);

            //Act
            //Assert

            await Assert.ThrowsAsync<NotFoundException>(async () => 
                await _fixture.MockFinancialService.DeleteSalaryByUserIdAsync(_fixture.UserId));
        }

        [Fact]
        public async Task GetListSalary_whenListIsNotEmpty_thanSucceed()
        {
            //Arrange

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo=> repo.GetAsync(
                    It.IsAny<SalaryParametrs>(),
                    It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(_fixture.SalaryList);

            //Act

            var result = await _fixture.MockFinancialService.GetSalaryAsync(_fixture.SalaryParametrs);

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            _fixture.MockLoggerManager.Verify();
            Assert.IsAssignableFrom<PagedList<Salary>>(result);
            Assert.Equal(_fixture.SalaryWithValue, result.Count());
        }

        [Fact]
        public async Task GetListSalary_whenListIsEmpty_thanSucceed()
        {
            //Arrange

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetAsync(
                    It.IsAny<SalaryParametrs>(),
                    It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(_fixture.SalaryEmptyList);

            //Act

            var result = await _fixture.MockFinancialService.GetSalaryAsync(_fixture.SalaryParametrs);

            //Assert

            Assert.NotNull(result);
            Assert.Empty(result);
            _fixture.MockLoggerManager.Verify();
            Assert.IsAssignableFrom<PagedList<Salary>>(result);

        }

        [Fact]
        public async Task GetSalaryByUserId_whenSalaryIsOk_thanSucceed()
        {
            //Arrange

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(
                    It.Is<int>(x=> x==_fixture.SalaryModel.EmployeeId),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.SalaryModel);

            //Act

            var result = await _fixture.MockFinancialService.GetSalaryByUserIdAsync(_fixture.SalaryModel.EmployeeId);

            //Assert

            Assert.NotNull(result);
            Assert.IsAssignableFrom<Salary>(result);
            _fixture.MockLoggerManager.Verify();
        }

        [Fact]
        public async Task GetSalaryByUserId_whenSalaryValueIsZero_thanNotFoundException()
        {
            //Arrange

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogWarn(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.SalaryWithZeroValue);

            //Act
            //Assert

            await Assert.ThrowsAsync<NotFoundException>(async()=> await _fixture.MockFinancialService
                .GetSalaryByUserIdAsync(_fixture.SalaryWithZeroValue.EmployeeId));
        }

        [Fact]
        public async Task GetSalaryByUserId_whenSalaryIsNotExist_thanNotFoundException()
        {
            //Arrange

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(
                    It.Is<int>(x=> x==_fixture.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(()=>null);

            //Act
            //Assert

            await Assert.ThrowsAsync<NotFoundException>(async () => await _fixture.MockFinancialService
                .GetSalaryByUserIdAsync(_fixture.UserId));
        }

        [Fact]
        public async Task UpdateSalary_whenSalaryIsExist_thanSucceed()
        {
            //Arrange

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(
                    It.Is<int>(x=> x==_fixture.UserId), 
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.SalaryModel);

            _fixture.MockSalaryRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //Act

            await _fixture.MockFinancialService.UpdateSalaryAsync(_fixture.UpdatedSalary);

            //Assert

            _fixture.MockLoggerManager.Verify();
            _fixture.MockSalaryRepository.Verify(repo => repo.GetById(_fixture.UserId, ""), Times.Once);
            _fixture.MockSalaryRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateSalary_whenNewValueIsEqualOld_thanBadRequestException()
        {
            //Arrange

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(
                    It.Is<int>(x=>x==_fixture.SalaryModel.EmployeeId), 
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.SalaryModel);

            //Act
            //Assert

            await Assert.ThrowsAsync<BadRequestException>(async () => 
                await _fixture.MockFinancialService.UpdateSalaryAsync(_fixture.SalaryModel));
        }

        [Fact]
        public async Task UpdateSalary_whenSalaryValueIsZero_thanNotFoundException()
        {
            //Arrange

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogWarn(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(
                    It.Is<int>(x=> x==_fixture.SalaryWithZeroValue.EmployeeId), 
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.SalaryWithZeroValue);

            //Act
            //Assert

            await Assert.ThrowsAsync<NotFoundException>(async () => 
                await _fixture.MockFinancialService.UpdateSalaryAsync(_fixture.SalaryModel));
        }

        [Fact]
        public async Task UpdateSalary_whenSalaryIsNotExist_thanNotFoundException()
        {
            //Arrange

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogWarn(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(
                    It.Is<int>(x=> x==_fixture.SalaryModel.EmployeeId), 
                    It.IsAny<string>()))
                .ReturnsAsync(() => null);

            //Act
            //Assert

            await Assert.ThrowsAsync<NotFoundException>(async () => 
                await _fixture.MockFinancialService.UpdateSalaryAsync(_fixture.SalaryModel));
        }

        [Fact] 
        public async Task GetListEmployeeWithoutSalary_whenItIsNotEmpty_thanSucceed()
        {
            //Arrange

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetAsync(
                    It.IsAny<SalaryParametrs>(),
                    It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(_fixture.SalaryList);

            _fixture.MockUserRepository
                .Setup(repo => repo.GetByRolesAsync(
                    It.IsAny<List<int>>(),
                    It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.EmployeeList);

            //Act

            var result = await _fixture.MockFinancialService.GetEmployeesWithoutSalary();

            //Assert

            _fixture.MockLoggerManager.Verify();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<IEnumerable<User>>(result);
        }

        //Financial Statement Tests
        [Fact]
        public async Task GetFinancialStatement_WhenDatePeriodIntervalIsLessThanMonth_thanReturAbsInterval()
        {
            //Arrange
            var date = new DatePeriod()
            {
                StartDate = new DateTime(2022, 5, 1),
                EndDate = new DateTime(2022, 5, 10)
            };
            var financialStatementParameters = _fixture.FinancialStatementParameters;
            financialStatementParameters.Date = date;

            //Act
            var result = await _fixture.MockFinancialService.GetFinancialStatement(financialStatementParameters);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0 ,result.TotalCount);
        }

        [Fact]
        public async Task GetFinancialStatement_WhenDatePeriodIntervalIsNotInt_thanReturnAbsInterval()
        {
            //Arrange
            var date = new DatePeriod()
            {
                StartDate = new DateTime(2022, 5, 1),
                EndDate = new DateTime(2022, 6, 10)
            };
            var financialStatementParameters = _fixture.FinancialStatementParameters;
            financialStatementParameters.Date = date;

            _fixture.MockAppointmentRepository
                .Setup(repo =>
                    repo.GetAsync(
                        It.IsAny<Expression<Func<Appointment, bool>>>(),
                        It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(_fixture.AppoinmentEmptyList);

            _fixture.MockSalaryRepository
                .Setup(repo=>
                    repo.GetAllForStatement(
                        It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(_fixture.SalaryEmptyList);

            //Act
            var result = await _fixture.MockFinancialService.GetFinancialStatement(financialStatementParameters);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task GetFinancialStatement_WhenDatePeriodIntervalIsInt_thanReturnThisInterval()
        {
            //Arrange

            _fixture.MockAppointmentRepository
                .Setup(repo =>
                    repo.GetAsync(
                        It.IsAny<Expression<Func<Appointment, bool>>>(),
                        It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(_fixture.AppoinmentEmptyList);

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(_fixture.SalaryEmptyList);

            //Act
            var result = await _fixture.MockFinancialService.GetFinancialStatement(_fixture.FinancialStatementParameters);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task GetFinancialStatement_whenDatePeriodIntervalIsGreaterThanOne_thanReturnMoreThanOneFinStatForMonth()
        {
            //Arrange
            var date = new DatePeriod()
            {
                StartDate = new DateTime(2022, 5, 1),
                EndDate = new DateTime(2022, 7, 1)
            };
            var financialStatementParameters = _fixture.FinancialStatementParameters;
            financialStatementParameters.Date = date;

            _fixture.MockAppointmentRepository
                .Setup(repo =>
                    repo.GetAsync(
                        It.IsAny<Expression<Func<Appointment, bool>>>(),
                        It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(_fixture.AppoinmentEmptyList);

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(_fixture.SalaryEmptyList);

            //Act
            var result = await _fixture.MockFinancialService.GetFinancialStatement(financialStatementParameters);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(2, result.TotalCount);
            Assert.IsAssignableFrom<PagedList<FinancialStatement>>(result);
        }

        [Fact]
        public async Task GetFinancialStatement_whenDatePeriodIsGreaterThanOneAndPageSizeIsOne_thanReturnOneFinStatForMonth()
        {
            //Arrange
            var date = new DatePeriod()
            {
                StartDate = new DateTime(2022, 5, 1),
                EndDate = new DateTime(2022, 7, 1)
            };
            var financialStatementParameters = _fixture.FinancialStatementParameters;
            financialStatementParameters.Date = date;
            financialStatementParameters.PageSize = 1;

            _fixture.MockAppointmentRepository
                .Setup(repo =>
                    repo.GetAsync(
                        It.IsAny<Expression<Func<Appointment, bool>>>(),
                        It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(_fixture.AppoinmentEmptyList);

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(_fixture.SalaryEmptyList);

            //Act
            var result = await _fixture.MockFinancialService.GetFinancialStatement(financialStatementParameters);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result);
            Assert.Equal(2, result.TotalCount);
            Assert.IsAssignableFrom<PagedList<FinancialStatement>>(result);

        }

        [Fact]
        public async Task GetFinacialStatement_whenThereIsOneAppoinment_thanReturnOk()
        {
            //Arrange
            var appoinmentList = new List<Appointment>()
            {
                new Appointment
                {
                    Id = 1,
                    AnimalId = 1,
                    Disease = "first",
                    AppointmentProcedures = new List<AppointmentProcedure>() 
                    { 
                        new AppointmentProcedure() 
                        { 
                            AppointmentId = 1, 
                            ProcedureId = 1 
                        } 
                    },
                    AppointmentUsers = new List<AppointmentUser>() 
                    { 
                        new AppointmentUser() 
                        { 
                            AppointmentId = 1, 
                            UserId = 1 
                        } 
                    }
                }
            };

            _fixture.MockAppointmentRepository
                .Setup(repo =>
                    repo.GetAsync(
                        It.IsAny<Expression<Func<Appointment, bool>>>(),
                        It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(appoinmentList);

            _fixture.MockProcedureRepository
                .Setup(repo =>
                    repo.GetById(
                        It.Is<int>(x=> x==_fixture.ProcedureOne.Id),
                        It.IsAny<string>()))
                .ReturnsAsync(_fixture.ProcedureOne)
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(_fixture.SalaryEmptyList);

            //Act
            var result = await _fixture.MockFinancialService.GetFinancialStatement(_fixture.FinancialStatementParameters);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<PagedList<FinancialStatement>>(result);
            _fixture.MockProcedureRepository.Verify(repo=> repo.GetById(_fixture.ProcedureOne.Id,""), Times.Once);
            _fixture.MockProcedureRepository.VerifyAll();
        }

        [Fact]
        public async Task GetFinacialStatement_whenThereIsBiggerThanOneAppoinment_thanReturnOk()
        {
            //Arrange

            _fixture.MockAppointmentRepository
                .Setup(repo =>
                    repo.GetAsync(
                        It.IsAny<Expression<Func<Appointment, bool>>>(),
                        It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(_fixture.AppointmentList);

            _fixture.MockProcedureRepository
                .Setup(repo =>
                    repo.GetById(
                        It.Is<int>(x => x == _fixture.ProcedureOne.Id),
                        It.IsAny<string>()))
                .ReturnsAsync(_fixture.ProcedureOne)
                .Verifiable();

            _fixture.MockProcedureRepository
                .Setup(repo =>
                    repo.GetById(
                        It.Is<int>(x => x == _fixture.ProcedureTwo.Id),
                        It.IsAny<string>()))
                .ReturnsAsync(_fixture.ProcedureTwo)
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(_fixture.SalaryEmptyList);

            //Act
            var result = await _fixture.MockFinancialService.GetFinancialStatement(_fixture.FinancialStatementParameters);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<PagedList<FinancialStatement>>(result);
            foreach(var res in result)
            {
                Assert.Equal(2, res.IncomesList.Count());
            }
            _fixture.MockProcedureRepository.VerifyAll();
        }

        [Fact]
        public async Task GetFinacialStatement_whenThereIsTwoProceduresInAppoinment_thanReturnOk()
        {
            //Arrange
            var appoinmentList = new List<Appointment>()
            {
                new Appointment
                {
                    Id = 1,
                    AnimalId = 1,
                    Disease = "first",
                    AppointmentProcedures = new List<AppointmentProcedure>()
                    {
                        new AppointmentProcedure()
                        {
                            AppointmentId = 1,
                            ProcedureId = 1
                        },
                        new AppointmentProcedure()
                        {
                            AppointmentId =1,
                            ProcedureId =2
                        }
                    },
                    AppointmentUsers = new List<AppointmentUser>()
                    {
                        new AppointmentUser()
                        {
                            AppointmentId = 1,
                            UserId = 1
                        }
                    }
                }
            };

            _fixture.MockAppointmentRepository
                .Setup(repo =>
                    repo.GetAsync(
                        It.IsAny<Expression<Func<Appointment, bool>>>(),
                        It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(appoinmentList);

            _fixture.MockProcedureRepository
                .Setup(repo =>
                    repo.GetById(
                        It.Is<int>(x => x == _fixture.ProcedureOne.Id),
                        It.IsAny<string>()))
                .ReturnsAsync(_fixture.ProcedureOne)
                .Verifiable();

            _fixture.MockProcedureRepository
                .Setup(repo =>
                    repo.GetById(
                        It.Is<int>(x => x == _fixture.ProcedureTwo.Id),
                        It.IsAny<string>()))
                .ReturnsAsync(_fixture.ProcedureTwo)
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(_fixture.SalaryEmptyList);

            //Act
            var result = await _fixture.MockFinancialService.GetFinancialStatement(_fixture.FinancialStatementParameters);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<PagedList<FinancialStatement>>(result);
            _fixture.MockProcedureRepository.Verify(repo => repo.GetById(_fixture.ProcedureOne.Id, ""), Times.Once);
            _fixture.MockProcedureRepository.Verify(repo => repo.GetById(_fixture.ProcedureTwo.Id, ""), Times.Once);
            _fixture.MockProcedureRepository.VerifyAll();
        }

        [Fact]
        public async Task GetFinancialStatement_whenThereIsOneUserWithConstantSalaryValue_thanItsSalaryValueEquelTheOnesInResult()
        {
            //Arrange
            var salaryList = new List<Salary>()
            {
                _fixture.SalaryOne
            };

            _fixture.MockAppointmentRepository
                .Setup(repo =>
                    repo.GetAsync(
                        It.IsAny<Expression<Func<Appointment, bool>>>(),
                        It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(_fixture.AppoinmentEmptyList);

            _fixture.MockUserRepository
                .Setup(repo =>
                    repo.GetByIdAsync(
                        It.IsAny<int>(),
                        It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.UserOne);

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(salaryList);

            //Act
            var result = await _fixture.MockFinancialService.GetFinancialStatement(_fixture.FinancialStatementParameters);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            foreach(var res in result)
            {
                Assert.Single(res.ExpencesList);
                foreach(var exp in res.ExpencesList)
                {
                    Assert.Equal(_fixture.SalaryOne.Value, exp.SalaryValue);
                }
            }
        }

        [Fact]
        public async Task GetFinancialStatement_whenThereIsTwoUsersWithConstantSalaryValue_thanTheirSalaryValueEquelTheOnesInResult()
        {
            //Arrange

            var salaryList = new List<Salary>
            {
                _fixture.SalaryOne,
                _fixture.SalaryTwo,
            };

            _fixture.MockAppointmentRepository
                .Setup(repo =>
                    repo.GetAsync(
                        It.IsAny<Expression<Func<Appointment, bool>>>(),
                        It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(_fixture.AppoinmentEmptyList);

            _fixture.MockUserRepository
                .Setup(repo =>
                    repo.GetByIdAsync(
                        It.Is<int>(x=> x==_fixture.UserOne.Id),
                        It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.UserOne);

            _fixture.MockUserRepository
                .Setup(repo =>
                    repo.GetByIdAsync(
                        It.Is<int>(x => x == _fixture.UserTwo.Id),
                        It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.UserTwo);

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(salaryList);

            //Act

            var result = await _fixture.MockFinancialService.GetFinancialStatement(_fixture.FinancialStatementParameters);

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            foreach (var res in result)
            {
                foreach (var exp in res.ExpencesList)
                {
                    if(exp.EmployeeName == _fixture.UserOne.FirstName + " " + _fixture.UserOne.LastName)
                        Assert.Equal(_fixture.SalaryOne.Value, exp.SalaryValue);

                    if (exp.EmployeeName == _fixture.UserTwo.FirstName + " " + _fixture.UserTwo.LastName)
                        Assert.Equal(_fixture.SalaryTwo.Value, exp.SalaryValue);
                }
            }
        }

        [Fact]
        public async Task GetFinancialStatement_whenThereIsANewUserDuringPeriod_thanExpencesListIncludeTwoEmployees()
        {
            //Arrange

            var firstSalaryList = new List<Salary>()
            {
                _fixture.SalaryOne
            };

            var newSalary = _fixture.SalaryTwo;
            newSalary.Date = new DateTime(2022,5,20);

            var secondSalaryList = new List<Salary>()
            {
                _fixture.SalaryOne,
                newSalary
            };

            Expression<Func<Salary, bool>> firstFilter = x => x.Date < _fixture.FinancialStatementParameters.Date.StartDate && x.Value != 0;
            Expression<Func<Salary, bool>> secondFilter = x => x.Date < _fixture.FinancialStatementParameters.Date.EndDate;
            Expression<Func<Salary, bool>> thirdFilter = x => x.Date > _fixture.FinancialStatementParameters.Date.StartDate;


            _fixture.MockAppointmentRepository
               .Setup(repo =>
                   repo.GetAsync(
                       It.IsAny<Expression<Func<Appointment, bool>>>(),
                       It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(_fixture.AppoinmentEmptyList);

            _fixture.MockUserRepository
                .Setup(repo =>
                    repo.GetByIdAsync(
                        It.Is<int>(x => x == _fixture.UserOne.Id),
                        It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.UserOne);

            _fixture.MockUserRepository
                .Setup(repo =>
                    repo.GetByIdAsync(
                        It.Is<int>(x => x == _fixture.UserTwo.Id),
                        It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.UserTwo);

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.Is<Expression<Func<Salary, bool>>>(expression=>
                            Lambda.Eq(expression,firstFilter))))
                .ReturnsAsync(firstSalaryList);

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.Is<Expression<Func<Salary, bool>>>(expression =>
                            Lambda.Eq(expression, secondFilter))))
                .ReturnsAsync(secondSalaryList);

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetByIdForStatement(
                        It.Is<int>(x => x == _fixture.UserTwo.Id),
                        It.Is<Expression<Func<Salary, bool>>>(expression=>
                            Lambda.Eq(expression,thirdFilter))))
                .ReturnsAsync(newSalary);

            //Act

            var result = await _fixture.MockFinancialService.GetFinancialStatement(_fixture.FinancialStatementParameters);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            foreach(var res in result)
            {
                Assert.Equal(2, res.ExpencesList.Count());
            }
            
        }

        [Fact]
        public async Task GetFinancialStatement_whenUserHasNoConstSalaryValue_thanReturnOk()
        {
            //Arrange

            var firstSalaryList = new List<Salary>()
            {
                _fixture.SalaryOne
            };

            var newSalary = new Salary
            {
                EmployeeId = 1,
                Value = 30,
                Date = new DateTime(2022, 5, 20)
            };

            Expression<Func<Salary, bool>> firstFilter = s => s.Date >= _fixture.SalaryOne.Date;
            Expression<Func<Salary, bool>> secondFilter = s => s.Date >= newSalary.Date;

            _fixture.MockAppointmentRepository
               .Setup(repo =>
                   repo.GetAsync(
                       It.IsAny<Expression<Func<Appointment, bool>>>(),
                       It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(_fixture.AppoinmentEmptyList);

            _fixture.MockUserRepository
                .Setup(repo =>
                    repo.GetByIdAsync(
                        It.Is<int>(x => x == _fixture.UserOne.Id),
                        It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.UserOne);

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetByIdForStatement(
                        It.Is<int>(x => x == _fixture.UserOne.Id),
                        It.Is<Expression<Func<Salary, bool>>>(expression =>
                            Lambda.Eq(expression, firstFilter))))
                .ReturnsAsync(newSalary)
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo=>
                    repo.GetByIdForStatement(
                        It.Is<int>(x=> x==_fixture.UserOne.Id),
                        It.Is<Expression<Func<Salary,bool>>>(expression=>
                            Lambda.Eq(expression,secondFilter))))
                .ReturnsAsync(()=>null)
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(firstSalaryList);


            //Act

            var result = await _fixture.MockFinancialService.GetFinancialStatement(_fixture.FinancialStatementParameters);

            //Assert

            Assert.NotNull(result);
            _fixture.MockSalaryRepository.VerifyAll();
        }

        [Fact]
        public async Task GetFinancialStatement_whenUserHasTheNextSalaryValueAfterPeriod_thanReturnOk()
        {
            //Arrange

            var firstSalaryList = new List<Salary>()
            {
                _fixture.SalaryOne
            };

            var newSalary = new Salary
            {
                EmployeeId = 1,
                Value = 30,
                Date = new DateTime(2022, 5, 20)
            };
            var lastSalary = new Salary()
            {
                EmployeeId = 1,
                Value = 30,
                Date = new DateTime(2022, 6, 20)
            };

            Expression<Func<Salary, bool>> firstFilter = s => s.Date >= _fixture.SalaryOne.Date;
            Expression<Func<Salary, bool>> secondFilter = s => s.Date >= newSalary.Date;

            _fixture.MockAppointmentRepository
               .Setup(repo =>
                   repo.GetAsync(
                       It.IsAny<Expression<Func<Appointment, bool>>>(),
                       It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(_fixture.AppoinmentEmptyList);

            _fixture.MockUserRepository
                .Setup(repo =>
                    repo.GetByIdAsync(
                        It.Is<int>(x => x == _fixture.UserOne.Id),
                        It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.UserOne);

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetByIdForStatement(
                        It.Is<int>(x => x == _fixture.UserOne.Id),
                        It.Is<Expression<Func<Salary, bool>>>(expression =>
                            Lambda.Eq(expression, firstFilter))))
                .ReturnsAsync(newSalary)
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetByIdForStatement(
                        It.Is<int>(x => x == _fixture.UserOne.Id),
                        It.Is<Expression<Func<Salary, bool>>>(expression =>
                            Lambda.Eq(expression, secondFilter))))
                .ReturnsAsync(lastSalary)
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(firstSalaryList);

            //Act

            var result = await _fixture.MockFinancialService.GetFinancialStatement(_fixture.FinancialStatementParameters);

            //Assert

            Assert.NotNull(result);
            _fixture.MockSalaryRepository.VerifyAll();
        }

        [Fact]
        public async Task GetFinancialStatement_whenUsersSecondSalaryStartedOnPeriodStartDate_thanReturnOk()
        {
            //Arrange

            var firstSalaryList = new List<Salary>()
            {
                _fixture.SalaryOne
            };

            var newSalary = new Salary
            {
                EmployeeId = 1,
                Value = 30,
                Date = new DateTime(2022, 5, 1)
            };

            Expression<Func<Salary, bool>> firstFilter = s => s.Date >= _fixture.SalaryOne.Date;
            Expression<Func<Salary, bool>> secondFilter = s => s.Date >= newSalary.Date;

            _fixture.MockAppointmentRepository
               .Setup(repo =>
                   repo.GetAsync(
                       It.IsAny<Expression<Func<Appointment, bool>>>(),
                       It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(_fixture.AppoinmentEmptyList);

            _fixture.MockUserRepository
                .Setup(repo =>
                    repo.GetByIdAsync(
                        It.Is<int>(x => x == _fixture.UserOne.Id),
                        It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.UserOne);

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetByIdForStatement(
                        It.Is<int>(x => x == _fixture.UserOne.Id),
                        It.Is<Expression<Func<Salary, bool>>>(expression =>
                            Lambda.Eq(expression, firstFilter))))
                .ReturnsAsync(newSalary)
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetByIdForStatement(
                        It.Is<int>(x => x == _fixture.UserOne.Id),
                        It.Is<Expression<Func<Salary, bool>>>(expression =>
                            Lambda.Eq(expression, secondFilter))))
                .ReturnsAsync(() => null)
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(firstSalaryList);


            //Act

            var result = await _fixture.MockFinancialService.GetFinancialStatement(_fixture.FinancialStatementParameters);

            //Assert

            Assert.NotNull(result);
            _fixture.MockSalaryRepository.VerifyAll();
        }

        [Fact]
        public async Task GetFinancialStatement_whenUserHasAtLeastThreeSalaryValueInPeriod_thanReturnOk()
        {
            //Arrange

            var firstSalaryList = new List<Salary>()
            {
                _fixture.SalaryOne
            };

            var newSalary = new Salary
            {
                EmployeeId = 1,
                Value = 30,
                Date = new DateTime(2022, 5, 15)
            };
            var lastSalary = new Salary
            {
                EmployeeId = 1,
                Value = 30,
                Date = new DateTime(2022, 5, 25)
            };


            Expression<Func<Salary, bool>> firstFilter = s => s.Date >= _fixture.SalaryOne.Date;
            Expression<Func<Salary, bool>> secondFilter = s => s.Date >= newSalary.Date;



            _fixture.MockAppointmentRepository
               .Setup(repo =>
                   repo.GetAsync(
                       It.IsAny<Expression<Func<Appointment, bool>>>(),
                       It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(_fixture.AppoinmentEmptyList);

            _fixture.MockUserRepository
                .Setup(repo =>
                    repo.GetByIdAsync(
                        It.Is<int>(x => x == _fixture.UserOne.Id),
                        It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.UserOne);

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetByIdForStatement(
                        It.Is<int>(x => x == _fixture.UserOne.Id),
                        It.Is<Expression<Func<Salary, bool>>>(expression =>
                            Lambda.Eq(expression, firstFilter))))
                .ReturnsAsync(newSalary)
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetByIdForStatement(
                        It.Is<int>(x => x == _fixture.UserOne.Id),
                        It.Is<Expression<Func<Salary, bool>>>(expression =>
                            Lambda.Eq(expression, secondFilter))))
                .ReturnsAsync(lastSalary)
                .Verifiable();

            _fixture.MockSalaryRepository
                .Setup(repo =>
                    repo.GetAllForStatement(
                        It.IsAny<Expression<Func<Salary, bool>>>()))
                .ReturnsAsync(firstSalaryList);


            //Act

            var result = await _fixture.MockFinancialService.GetFinancialStatement(_fixture.FinancialStatementParameters);

            //Assert

            Assert.NotNull(result);
            _fixture.MockSalaryRepository.VerifyAll();
        }
    }
}
