using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.Paginator;
using Core.Paginator.Parameters;
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

            _id = 1;
            _exceptionEntity = new()
            {
                Id = _id,
                Name = "NotFoundException",
                DateTime = DateTime.Now,
                StackTrace = @" at Application.Services.ExceptionEntityService.GetAsync(Int32 id)",
                Path = @"/api/exceptions/1"
            };

            _exceptionEntityReadViewModel = new()
            {
                Id = _id,
                Name = "NotFoundException",
                DateTime = DateTime.Now,
                Path = @"/api/exceptions/1"
            };
            _exceptionStat = new()
            {
                Count = 1,
                Name = "NotFoundException"
            };
            _exceptionStats = new() { _exceptionStat };
            _exceptionEntities = new() { _exceptionEntity };
            _exceptionEntityReadViewModels = new() { _exceptionEntityReadViewModel };
            _pagedListExceptionStats = PagedList<ExceptionStats>.ToPagedList(_exceptionStats.AsQueryable(), 1, 10);
            _pagedListExceptions = PagedList<ExceptionEntity>.ToPagedList(_exceptionEntities.AsQueryable(), 1, 10);
            _pagedListViewModelsExceptions = PagedList<ExceptionEntityReadViewModel>.ToPagedList(_exceptionEntityReadViewModels.AsQueryable(), 1, 10);
            pagingParameters = new();
        }


        public ExceptionController MockExceptionController { get; set; }
        public int _id;
        public ExceptionEntity _exceptionEntity;
        public ExceptionEntityReadViewModel _exceptionEntityReadViewModel;
        public ExceptionStats _exceptionStat;

        public List<ExceptionStats> _exceptionStats;
        public List<ExceptionEntity> _exceptionEntities;
        public List<ExceptionEntityReadViewModel> _exceptionEntityReadViewModels;
        public PagedList<ExceptionStats> _pagedListExceptionStats;
        public PagedList<ExceptionEntity> _pagedListExceptions;
        public PagedList<ExceptionEntityReadViewModel> _pagedListViewModelsExceptions;
        public ExceptionParameters pagingParameters;
        public Mock<IExceptionEntityService> MockExceptionService { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>>> MockMapperException { get; }
    }
}
