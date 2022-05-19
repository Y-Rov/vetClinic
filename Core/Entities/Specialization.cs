namespace Core.Entities
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Procedure> Procedures { get; set; }
        public ICollection<User> Doctors { get; set; }
    }
}
