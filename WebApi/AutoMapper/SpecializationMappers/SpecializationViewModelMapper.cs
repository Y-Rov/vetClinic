using Core.Entities;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SpecializationViewModels;
using Core.ViewModels.User;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SpecializationMappers
{
    public class SpecializationViewModelMapper : IViewModelMapper<Specialization, SpecializationViewModel>
    {
        private ProcedureReadViewModel MapProcedure(Procedure procedure)
        {
            return new ProcedureReadViewModel
            {
                Id = procedure.Id,  
                Name = procedure.Name,
                DurationInMinutes = procedure.DurationInMinutes,
                Cost = procedure.Cost,
                Description = procedure.Description
            };
        }

        private UserBaseViewModel MapUser(User user)
        {
            return new UserBaseViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate
            };
        }

        public SpecializationViewModel Map(Specialization source)
        {
            return new SpecializationViewModel()
            {
                Id = source.Id,
                Name = source.Name,
                Procedures =
                    source.ProcedureSpecializations
                    .Select(ps => MapProcedure(ps.Procedure)).ToList(),
                Users =
                    source.UserSpecializations?
                    .Select(us => MapUser(us.User)).ToList()
            };
        }
    }
}
