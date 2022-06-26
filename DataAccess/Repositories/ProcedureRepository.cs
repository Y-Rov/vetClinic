using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories;

public class ProcedureRepository : Repository<Procedure>, IProcedureRepository
{
    private readonly IProcedureSpecializationRepository _procedureSpecializationRepository;

    public ProcedureRepository(ClinicContext clinicContext) : base(clinicContext) { }
    
}