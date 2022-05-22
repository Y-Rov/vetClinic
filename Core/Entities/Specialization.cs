namespace Core.Entities
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProcedureSpecialization> ProcedureSpecializations { get; set; }
        public ICollection<UserSpecialization> UserSpecializations { get; set; }
    }
}
