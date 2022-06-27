using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Pagginator;
using Core.Pagginator.Parameters;
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
        }

        public ExceptionEntityService MockExceptionEntityService { get; }
        public Mock<IExceptionEntityRepository> MockExceptionRepository { get; }
        public Mock<ILoggerManager> MockLogger { get; }
        public static readonly int _id = 1;

        public static readonly ExceptionEntity _exceptionEntity = new()
        {
            Id = _id,
            Name = "NotFoundException",
            DateTime = DateTime.Now,
            StackTrace = @" at Application.Services.ExceptionEntityService.GetAsync(Int32 id)",
            Path = @"/api/exceptions/1"
        };

        public static readonly ExceptionStats _exceptionStat = new()
        {
            Count = 1,
            Name = "NotFoundException"
        };

        public static readonly List<ExceptionStats> _exceptionStats = new() { _exceptionStat };
        public static readonly List<ExceptionEntity> _exceptionEntities = new() { _exceptionEntity };
        public static readonly PagedList<ExceptionStats> _paggedListExceptionStats = PagedList<ExceptionStats>.ToPagedList(_exceptionStats.AsQueryable(), 1, 10);
        public static readonly PagedList<ExceptionEntity> _paggedListExceptions = PagedList<ExceptionEntity>.ToPagedList(_exceptionEntities.AsQueryable(), 1, 10);
        public static readonly ExceptionParameters paggingParameters = new()
        {
            PageNumber = 1,
            PageSize = 10,
        };
    }

}
