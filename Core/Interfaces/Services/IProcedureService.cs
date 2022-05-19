using Core.Entities;

namespace Core.Interfaces.Services;

public interface IProcedureService
{
    public void CreateNewProcedure(Procedure procedure);
    public void UpdateProcedure(int oldProcedureId, Procedure newProcedure);
    public void DeleteProcedure(int procedureId);

    public Procedure GetById(int procedureId);
    
    public IEnumerable<Procedure> GetAllProcedures();
}