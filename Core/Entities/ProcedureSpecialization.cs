namespace Core.Entities;

public class ProcedureSpecialization
{
    public int ProcedureId { get; set; }

    public Procedure Procedure { get; set; }
    
    public int SpecializationId { get; set; }

    public Specialization Specialization { get; set; }
}