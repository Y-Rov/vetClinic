using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories;

public class ProcedureSpecializationRepository : 
    Repository<ProcedureSpecialization>, 
    IProcedureSpecializationRepository

{
    public ProcedureSpecializationRepository(ClinicContext context) : base(context)
    {
    }
}