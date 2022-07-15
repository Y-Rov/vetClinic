using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Core.Paginator;
using Core.Paginator.Parameters;
using Microsoft.EntityFrameworkCore.Query;
using Core.Models.Finance;
using Moq;
using System.Linq.Expressions;

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
                    It.IsAny<Expression<Func<Salary, bool>>>(),
                    It.IsAny<Func<IQueryable<Salary>, IOrderedQueryable<Salary>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(_fixture.SalaryListFromRepo);

            //Act

            var result = await _fixture.MockFinancialService.GetSalaryAsync(null);

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            _fixture.MockLoggerManager.Verify();
            Assert.IsAssignableFrom<IEnumerable<Salary>>(result);
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
                    It.IsAny<Expression<Func<Salary, bool>>>(),
                    It.IsAny<Func<IQueryable<Salary>, IOrderedQueryable<Salary>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(_fixture.SalaryEmptyList);

            //Act

            var result = await _fixture.MockFinancialService.GetSalaryAsync(null);

            //Assert

            Assert.NotNull(result);
            Assert.Empty(result);
            _fixture.MockLoggerManager.Verify();
            Assert.IsAssignableFrom<IEnumerable<Salary>>(result);

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
                    It.IsAny<Expression<Func<Salary, bool>>>(),
                    It.IsAny<Func<IQueryable<Salary>, IOrderedQueryable<Salary>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(_fixture.SalaryListFromRepo);

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

        [Fact]
        public async Task GetFinancialStatement_WhenAppoinmentsExist_thenReturnOk()
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
                        It.Is<int>(x=> x==_fixture.ProcedureOne.Id),
                        It.IsAny<string>()))
                .ReturnsAsync(_fixture.ProcedureOne);

            _fixture.MockProcedureRepository
                .Setup(repo =>
                     repo.GetById(
                        It.Is<int>(x => x == _fixture.ProcedureTwo.Id),
                        It.IsAny<string>()))
                .ReturnsAsync(_fixture.ProcedureTwo);

            _fixture.MockSalaryRepository
                .Setup(repo
                    => repo.GetAsync(
                        It.IsAny<Expression<Func<Salary, bool>>>(),
                        It.IsAny<Func<IQueryable<Salary>, IOrderedQueryable<Salary>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(_fixture.SalaryList);

            _fixture.MockUserRepository
                .Setup(repo=>
                    repo.GetByIdAsync(
                        It.Is<int>(x=> x.Equals(_fixture.UserOne.Id)),
                        It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.UserOne);

            _fixture.MockUserRepository
                .Setup(repo =>
                    repo.GetByIdAsync(
                        It.Is<int>(x => x.Equals(_fixture.UserTwo.Id)),
                        It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.UserTwo);

            //Act

            var result = await _fixture.MockFinancialService
                .GetFinancialStatement(_fixture.Date,_fixture.FinancialStatementParameters);
            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<IEnumerable<FinancialStatement>>(result);
        }

        [Fact]
        public async Task GetFinancialStatement_WhenAppoinmentsNotExist_thenReturnOk()
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
                .Setup(repo
                    => repo.GetAsync(
                        It.IsAny<SalaryParametrs>(),
                        It.IsAny<Expression<Func<Salary, bool>>>(),
                        It.IsAny<Func<IQueryable<Salary>, IOrderedQueryable<Salary>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<string>()))
                .ReturnsAsync(_fixture.SalaryList);

            _fixture.MockUserRepository
                .Setup(repo =>
                    repo.GetByIdAsync(
                        It.Is<int>(x => x.Equals(_fixture.UserOne.Id)),
                        It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.UserOne);

            _fixture.MockUserRepository
                .Setup(repo =>
                    repo.GetByIdAsync(
                        It.Is<int>(x => x.Equals(_fixture.UserTwo.Id)),
                        It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.UserTwo);

            //Act

            var result = await _fixture.MockFinancialService
                .GetFinancialStatement(_fixture.Date, _fixture.FinancialStatementParameters);

            //Assert

            Assert.NotNull(result);
            Assert.IsAssignableFrom<PagedList<FinancialStatement>>(result);
        }
    }
}
