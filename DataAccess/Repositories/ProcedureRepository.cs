using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories;

public class ProcedureRepository : Repository<Procedure>, IProcedureRepository
{
    private readonly IProcedureSpecializationRepository _procedureSpecializationRepository;

    public ProcedureRepository(
        ClinicContext clinicContext, 
        IProcedureSpecializationRepository procedureSpecializationRepository) 
        : base(clinicContext)
    {
        _procedureSpecializationRepository = procedureSpecializationRepository;
    }

    public async Task UpdateProcedureSpecializationsAsync(int procedureId, IEnumerable<int> specializationIds)
    {
        var existing = await _procedureSpecializationRepository.GetAsync(
            filter: pr => pr.ProcedureId == procedureId);
        foreach (var ps in existing)
        {
            _procedureSpecializationRepository.Delete(ps);
        }
        await _procedureSpecializationRepository.SaveChangesAsync();

        foreach (var specializationId in specializationIds)
        {
            await _procedureSpecializationRepository.InsertAsync(new ProcedureSpecialization()
            {
                ProcedureId = procedureId,
                SpecializationId = specializationId
            });
        }
        await SaveChangesAsync();
    }

}