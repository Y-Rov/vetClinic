using Core.Entities;
using Core.Interfaces.Services;
using DataAccess.Context;

namespace Application.Services;

public class ProcedureService : IProcedureService
{
    private readonly ClinicContext _clinicContext;
    public ProcedureService(ClinicContext clinicContext)
    {
        _clinicContext = clinicContext;
    }

    public Task CreateNewProcedureAsync(Procedure procedure)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProcedureAsync(int oldProcedureId, Procedure newProcedure)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProcedureAsync(int procedureId)
    {
        throw new NotImplementedException();
    }

    public Task<Procedure> GetByIdAsync(int procedureId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Procedure>> GetAllProceduresAsync()
    {
        throw new NotImplementedException();
    }
}