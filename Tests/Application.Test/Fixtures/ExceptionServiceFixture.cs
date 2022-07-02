using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Extensions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Paginator;
using Core.Paginator.Parameters;
using Moq;

namespace Application.Test.Fixtures
{
    public class ExceptionServiceFixture
    {
        public ExceptionServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockExceptionEntityService = fixture.Freeze<ExceptionEntityService>();
            MockExceptionRepository = fixture.Freeze<Mock<IExceptionEntityRepository>>();
            MockLogger = fixture.Freeze<Mock<ILoggerManager>>();

            MockExceptionEntityService = new ExceptionEntityService(
                MockExceptionRepository.Object,
                MockLogger.Object);

            _id = 1;
            _exceptionEntity = new()
            {
                Id = _id,
                Name = "NotFoundException",
                DateTime = DateTime.Now,
                StackTrace = @" at Application.Services.ExceptionEntityService.GetAsync(Int32 id)",
                Path = @"/api/exceptions/1"
            };
            _exceptionStat = new()
            {
                Count = 1,
                Name = "NotFoundException"
            };
            _exceptionStats = new() { _exceptionStat };
            _exceptionEntities = new() { _exceptionEntity };
            _pagedListExceptionStats = PagedList<ExceptionStats>.ToPagedList(_exceptionStats.AsQueryable(), 1, 10);
            _pagedListExceptions = PagedList<ExceptionEntity>.ToPagedList(_exceptionEntities.AsQueryable(), 1, 10);
            pagingParameters = new()
            {
                PageNumber = 1,
                PageSize = 10,
            };
        }

        public ExceptionEntityService MockExceptionEntityService { get; }
        public Mock<IExceptionEntityRepository> MockExceptionRepository { get; }
        public Mock<ILoggerManager> MockLogger { get; }
        public int _id;

        public ExceptionEntity _exceptionEntity;

        public ExceptionStats _exceptionStat;

        public List<ExceptionStats> _exceptionStats;
        public List<ExceptionEntity> _exceptionEntities;
        public PagedList<ExceptionStats> _pagedListExceptionStats;
        public PagedList<ExceptionEntity> _pagedListExceptions;
        public ExceptionParameters pagingParameters;
    }

}
