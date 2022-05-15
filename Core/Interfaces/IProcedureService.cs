namespace Core.Interfaces;

public interface IProcedureService
{
    public void CreateNewProcedure();
    public void UpdateProcedure();
    public void DeleteProcedure();
    public IEnumerable<Procedure> GetProcedures();
}