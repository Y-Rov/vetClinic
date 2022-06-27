using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.Pagginator;
using Core.Pagginator.Parameters;
using Core.ViewModel;
using Moq;
using WebApi.AutoMapper.Interface;
using WebApi.Controllers;

namespace WebApi.Test.Fixtures
{
    public class ExceptionControllerFixture
    {
        public ExceptionControllerFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockExceptionService = fixture.Freeze<Mock<IExceptionEntityService>>();
            MockMapperException = fixture.Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>>>>();

            MockExceptionController = new ExceptionController(
                MockExceptionService.Object,
                MockMapperException.Object);
        }


        public ExceptionController MockExceptionController { get; set; }
        public static readonly int _id = 1;
        public static readonly ExceptionEntity _exceptionEntity = new()
        {
            Id = _id,
            Name = "NotFoundException",
            DateTime = DateTime.Now,
            StackTrace = @" at Application.Services.ExceptionEntityService.GetAsync(Int32 id)",
            Path = @"/api/exceptions/1"
        };
        public static readonly ExceptionEntityReadViewModel _exceptionEntityReadViewModel = new()
        {
            Id = _id,
            Name = "NotFoundException",
            DateTime = DateTime.Now,
            Path = @"/api/exceptions/1"
        };
        public static readonly ExceptionStats _exceptionStat = new()
        {
            Count = 1,
            Name = "NotFoundException"
        };

        public static readonly List<ExceptionStats> _exceptionStats = new() { _exceptionStat };
        public static readonly List<ExceptionEntity> _exceptionEntities = new() { _exceptionEntity };
        public static readonly List<ExceptionEntityReadViewModel> _exceptionEntityReadViewModels = new() { _exceptionEntityReadViewModel };
        public static readonly PagedList<ExceptionStats> _pagedListExceptionStats = PagedList<ExceptionStats>.ToPagedList(_exceptionStats.AsQueryable(), 1, 10);
        public static readonly PagedList<ExceptionEntity> _pagedListExceptions = PagedList<ExceptionEntity>.ToPagedList(_exceptionEntities.AsQueryable(), 1, 10);
        public static readonly PagedList<ExceptionEntityReadViewModel> _pagedListViewModelsExceptions = PagedList<ExceptionEntityReadViewModel>.ToPagedList(_exceptionEntityReadViewModels.AsQueryable(), 1, 10);
        public static readonly ExceptionParameters pagingParameters = new();
        public Mock<IExceptionEntityService> MockExceptionService { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>>> MockMapperException { get; }
    }
}
