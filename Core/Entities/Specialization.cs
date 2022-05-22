namespace Core.Entities
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserSpecialization> UserSpecializations { get; set; }
    }
}
