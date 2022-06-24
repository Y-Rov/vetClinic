using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
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

        public ExceptionController MockExceptionController { get; }
        public Mock<IExceptionEntityService> MockExceptionService { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>>> MockMapperException { get; }
    }
}
