namespace Core.Entities;

public class Procedure
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Cost { get; set; }
    public int DurationInMinutes { get; set; }
    public IEnumerable<AppointmentProcedure> AppointmentProcedures { get; set; } = new List<AppointmentProcedure>();
    public IEnumerable<Specialization>? Specializations { get; set;}
}

