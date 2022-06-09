using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories;

public class ProcedureRepository : Repository<Procedure>, IProcedureRepository
{
    private readonly ClinicContext _clinicContext;
    private readonly ISpecializationRepository _specializationRepository;

    public ProcedureRepository(
        ClinicContext clinicContext, 
        ISpecializationRepository specializationRepository) : base(clinicContext)
    {
        _clinicContext = clinicContext;
        _specializationRepository = specializationRepository;
    }

    public async Task UpdateProcedureSpecializationsAsync(Procedure procedure, IEnumerable<int> specializationIds)
    {
        _clinicContext.ProcedureSpecializations.RemoveRange(procedure.ProcedureSpecializations);
        await SaveChangesAsync();
        
        foreach (var specializationId in specializationIds)
        {
            await _specializationRepository.GetById(specializationId);
            await _clinicContext.ProcedureSpecializations.AddAsync(new ProcedureSpecialization()
            {
                ProcedureId = procedure.Id,
                SpecializationId = specializationId
            });
        }
    }
}