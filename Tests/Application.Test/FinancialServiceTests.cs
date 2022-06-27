using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Moq;
using System.Linq.Expressions;

namespace Application.Test
{
    public class FinancialServiceTests: IClassFixture<FinancialServiceFixture>
    {
        public FinancialServiceTests(FinancialServiceFixture fixture)
        {
            _fixture = fixture;
        }

        readonly FinancialServiceFixture _fixture;

        [Fact]
        public async Task CreateSalary_whenSalaryIsNotExist_thanCreate()
        {
            //Arrange
            var salary = new Salary()
            {
                Id = 1,
                EmployeeId = 1,
                Value = 5
            };

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new Salary());
            _fixture.MockSalaryRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            _fixture.MockSalaryRepository
                .Setup(repo=>repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            await _fixture.MockFinancialService.CreateSalaryAsync(salary);

            //Assert
            _fixture.MockSalaryRepository.Verify();
            
        }

        [Fact]
        public async Task CreateSalary_whenSalaryIsExist_thanBadRequestException()
        {
            //Arrange
            var salary = new Salary()
            {
                Id = 1,
                EmployeeId = 1,
                Value = 5
            };

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(salary);
            _fixture.MockSalaryRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            _fixture.MockSalaryRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            //Assert
            await Assert.ThrowsAsync<BadRequestException>(async()=> await _fixture.MockFinancialService.CreateSalaryAsync(salary));

        }

        [Fact]
        public async Task DeleteSalary_whenSalaryIsExist_thanDelete()
        {
            //Arrange

            var salary = new Salary
            {
                Id = 1,
                EmployeeId = 1,
                Value = 10
            };

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(salary);
            _fixture.MockSalaryRepository
                .Setup(repo=> repo.InsertAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            _fixture.MockSalaryRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            //Act
            await _fixture.MockFinancialService.DeleteSalaryByUserIdAsync(salary.EmployeeId);

            //Assert

            _fixture.MockSalaryRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteSalary_whenSalaryValueIsZero_thanNotFoundException()
        {
            //Arrange

            var salary = new Salary
            {
                Id = 1,
                EmployeeId = 1,
                Value = 0
            };

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(salary);
            _fixture.MockSalaryRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            _fixture.MockSalaryRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            //Act
            

            //Assert

            await Assert.ThrowsAsync<NotFoundException>(async() => await _fixture.MockFinancialService.DeleteSalaryByUserIdAsync(salary.EmployeeId));
        }

        [Fact]
        public async Task DeleteSalary_whenSalaryIsNotExist_thanNotFoundException()
        {
            //Arrange
            int id = 1;
            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(()=>null);
            _fixture.MockSalaryRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            _fixture.MockSalaryRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            //Act


            //Assert

            await Assert.ThrowsAsync<NotFoundException>(async () => await _fixture.MockFinancialService.DeleteSalaryByUserIdAsync(id));
        }

        [Fact]
        public async Task GetListSalary_whenListIsNotEmpty_thanSucceed()
        {
            //Arrange
            int salariesWithValue = 2;
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
                    Value = 0
                },
                new Salary()
                {
                    Id = 3,
                    EmployeeId = 3,
                    Value = 30
                }
            };

            _fixture.MockSalaryRepository
                .Setup(repo=> repo.GetAsync(
                    It.IsAny<Expression<Func<Salary, bool>>>(),
                    It.IsAny<Func<IQueryable<Salary>, IOrderedQueryable<Salary>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(salaryList);

            //Act
            var result = await _fixture.MockFinancialService.GetSalaryAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(salariesWithValue, result.Count());
        }

        [Fact]
        public async Task GetListSalary_whenListIsEmpty_thanSucceed()
        {
            //Arrange
            var emptysalaryList = new List<Salary>();

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetAsync(
                    It.IsAny<Expression<Func<Salary, bool>>>(),
                    It.IsAny<Func<IQueryable<Salary>, IOrderedQueryable<Salary>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(emptysalaryList);

            //Act
            var result = await _fixture.MockFinancialService.GetSalaryAsync();

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetSalaryByUserId_whenSalaryIsOk_thanSucceed()
        {
            //Arrange
            var salary = new Salary()
            {
                Id = 1,
                EmployeeId = 1,
                Value = 10
            };

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(salary);

            //Act
            var result = await _fixture.MockFinancialService.GetSalaryByUserIdAsync(salary.EmployeeId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(salary, result);
        }

        [Fact]
        public async Task GetSalaryByUserId_whenSalaryValueIsZero_thanNotFoundException()
        {
            //Arrange
            var salaryWithValueIsZero = new Salary()
            {
                Id = 1,
                EmployeeId = 1,
                Value = 0
            };

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(salaryWithValueIsZero);

            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async()=> await _fixture.MockFinancialService
                .GetSalaryByUserIdAsync(salaryWithValueIsZero.EmployeeId));
        }

        [Fact]
        public async Task GetSalaryByUserId_whenSalaryIsNotExist_thanNotFoundException()
        {
            int id = 1;
            //Arrange
            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(()=>null);

            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await _fixture.MockFinancialService
                .GetSalaryByUserIdAsync(id));
        }

        [Fact]
        public async Task UpdateSalary_whenSalaryIsExist_thanSucceed()
        {
            //Arrange

            var salary = new Salary
            {
                Id = 1,
                EmployeeId = 1,
                Value = 10
            };

            var oldSalary = new Salary 
            { 
               Id = 1, 
               EmployeeId = 1,
               Value = 20
            };

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(salary);
            _fixture.MockSalaryRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            _fixture.MockSalaryRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            //Act
            await _fixture.MockFinancialService.UpdateSalaryAsync(oldSalary);

            //Assert

            _fixture.MockSalaryRepository.VerifyAll();
        }

        [Fact]
        public async Task UpdateSalary_whenSalaryValueIsZero_thanNotFoundException()
        {
            //Arrange

            var salary = new Salary
            {
                Id = 1,
                EmployeeId = 1,
                Value = 0
            };

            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(salary);
            _fixture.MockSalaryRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            _fixture.MockSalaryRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            //Act


            //Assert

            await Assert.ThrowsAsync<NotFoundException>(async () => await _fixture.MockFinancialService
                .UpdateSalaryAsync(salary));
        }

        [Fact]
        public async Task UpdateSalary_whenSalaryIsNotExist_thanNotFoundException()
        {
            //Arrange
            var salary = new Salary
            {
                Id = 1,
                EmployeeId = 1,
                Value = 10
            };
            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);
            _fixture.MockSalaryRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Salary>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            _fixture.MockSalaryRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            //Act


            //Assert

            await Assert.ThrowsAsync<NotFoundException>(async () => await _fixture.MockFinancialService
                .UpdateSalaryAsync(salary));
        }

        [Fact] //don`t sure how to test
        public async Task GetListEmployeeWithoutSalary_whenItIsEmpty_thanSucceed()
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
                    Value = 0
                },
                new Salary()
                {
                    Id = 3,
                    EmployeeId = 3,
                    Value = 30
                }
            };
            var employeesId = new List<int>()
            {
                1,2,3
            };

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


            _fixture.MockSalaryRepository
                .Setup(repo => repo.GetAsync(
                    It.IsAny<Expression<Func<Salary, bool>>>(),
                    It.IsAny<Func<IQueryable<Salary>, IOrderedQueryable<Salary>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(salaryList);
            _fixture.MockSalaryRepository
                .Setup(repo=> repo.GetEmployees())
                .ReturnsAsync(employeesId);
            _fixture.MockUserRepository
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(listEmployees);

            //Act
            var result = await _fixture.MockFinancialService.GetEmployeesWithoutSalary();

            //Assert
            Assert.NotNull(result);
        }
    }
}
