using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SpecializationViewModels;
using Moq;
using WebApi.AutoMapper.Interface;
using WebApi.Controllers;

namespace WebApi.Test.Fixtures
{
    public class SpecializationControllerFixture
    {
        public SpecializationControllerFixture()
        {
            var fixture = 
                new Fixture().Customize(new AutoMoqCustomization());

            MockSpecializationService = fixture.Freeze<Mock<ISpecializationService>>();

            MockMapperSpecialization = fixture.Freeze<Mock<IViewModelMapper<SpecializationViewModel, Specialization>>>();

            MockMapperSpecializationViewModel = fixture.Freeze<Mock<IViewModelMapper<Specialization,SpecializationViewModel>>>();

            MockMapperListSpecializationViewModel = fixture.Freeze<Mock<IViewModelMapper<IEnumerable<Specialization>, IEnumerable<SpecializationViewModel>>>>();

            MockMapperListProcedureViewModel = fixture.Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>>>>();

           MockController = new SpecializationController(
                MockSpecializationService.Object,
                MockMapperSpecialization.Object,
                MockMapperSpecializationViewModel.Object,
                MockMapperListSpecializationViewModel.Object,
                MockMapperListProcedureViewModel.Object
                );
        }

        public SpecializationController MockController { get; }
        public Mock<ISpecializationService> MockSpecializationService { get; }
        public Mock<IViewModelMapper<SpecializationViewModel, Specialization>> MockMapperSpecialization { get; }
        public Mock<IViewModelMapper<Specialization,SpecializationViewModel>> MockMapperSpecializationViewModel { get; }
        public Mock<IViewModelMapper<IEnumerable<Specialization>, IEnumerable<SpecializationViewModel>>> MockMapperListSpecializationViewModel { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>>> MockMapperListProcedureViewModel { get; }
    }
}
