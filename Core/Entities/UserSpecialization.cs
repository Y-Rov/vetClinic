namespace Core.Entities
{
    public class UserSpecialization
    {
        public int UserId { get; set; }
        public User? User { get; set; }
        public int SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }
    }
}
